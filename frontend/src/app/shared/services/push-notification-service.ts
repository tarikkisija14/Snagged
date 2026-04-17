import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from, of } from 'rxjs';
import { switchMap, catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PushNotificationService {
  private readonly apiUrl = `${environment.apiUrl}/pushsubscription`;

  constructor(private http: HttpClient) {}

  registerServiceWorker(): Observable<ServiceWorkerRegistration | null> {
    if (!('serviceWorker' in navigator)) return of(null);
    return from(navigator.serviceWorker.register('/service-worker.js')).pipe(
      catchError(() => of(null))
    );
  }

  get permissionState(): NotificationPermission {
    return 'Notification' in window ? Notification.permission : 'denied';
  }

  requestPermission(): Observable<NotificationPermission> {
    if (!('Notification' in window)) return of('denied' as NotificationPermission);
    return from(Notification.requestPermission());
  }

  subscribe(): Observable<boolean> {
    if (!('serviceWorker' in navigator) || !('PushManager' in window)) return of(false);

    // Use the VAPID key directly from environment — no need to wrap in of().
    const publicKey = environment.vapidPublicKey;

    return from(navigator.serviceWorker.ready).pipe(
      switchMap(reg =>
        from(reg.pushManager.subscribe({
          userVisibleOnly: true,
          applicationServerKey: this.urlBase64ToUint8Array(publicKey)
        }))
      ),
      switchMap(sub => {
        const json = sub.toJSON();
        const keys = json.keys as { p256dh: string; auth: string };
        return this.http.post(`${this.apiUrl}/subscribe`, {
          endpoint: sub.endpoint,
          p256DhKey: keys.p256dh,
          authKey: keys.auth
        }).pipe(map(() => true));
      }),
      catchError(() => of(false))
    );
  }

  unsubscribe(): Observable<boolean> {
    if (!('serviceWorker' in navigator)) return of(false);

    return from(navigator.serviceWorker.ready).pipe(
      switchMap(reg => from(reg.pushManager.getSubscription())),
      switchMap(sub => {
        if (!sub) return of(true);
        return this.http.post(`${this.apiUrl}/unsubscribe`, { endpoint: sub.endpoint }).pipe(
          switchMap(() => from(sub.unsubscribe())),
          map(() => true)
        );
      }),
      catchError(() => of(false))
    );
  }

  // Checks subscription status using the browser's PushManager directly.
  // The browser already knows whether a subscription exists — no backend
  // round-trip is needed here. The backend /status endpoint exists for
  // admin/debug tooling, not for the client toggle UI.
  isSubscribed(): Observable<boolean> {
    if (!('serviceWorker' in navigator) || !('PushManager' in window)) return of(false);

    return from(navigator.serviceWorker.ready).pipe(
      switchMap(reg => from(reg.pushManager.getSubscription())),
      map(sub => sub !== null),
      catchError(() => of(false))
    );
  }

  isPushSupported(): boolean {
    return 'serviceWorker' in navigator
      && 'PushManager' in window
      && 'Notification' in window;
  }


  private urlBase64ToUint8Array(base64String: string): Uint8Array<ArrayBuffer> {
    const padding = '='.repeat((4 - (base64String.length % 4)) % 4);
    const base64  = (base64String + padding).replace(/-/g, '+').replace(/_/g, '/');
    const rawData = window.atob(base64);
    const buffer  = new ArrayBuffer(rawData.length);
    const view    = new Uint8Array(buffer);
    for (let i = 0; i < rawData.length; i++) {
      view[i] = rawData.charCodeAt(i);
    }
    return view;
  }
}
