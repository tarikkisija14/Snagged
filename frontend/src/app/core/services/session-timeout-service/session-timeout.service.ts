import { Injectable, NgZone, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, Subscription, timer } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import {AuthService} from '../auth-service/AuthService';

export interface SessionTimeoutEvent {
  remainingSeconds: number;
}

@Injectable({ providedIn: 'root' })
export class SessionTimeoutService implements OnDestroy {


  readonly showWarning$ = new Subject<SessionTimeoutEvent>();

  readonly sessionExpired$ = new Subject<void>();

  private readonly WARNING_BEFORE_MS = 2 * 60 * 1000;
  private readonly CHECK_INTERVAL_MS = 15_000;

  private checkSub?: Subscription;
  private authSub?: Subscription;
  private warningShown = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private ngZone: NgZone,
  ) {
    this.authSub = this.authService.currentUser$.subscribe(userId => {
      if (userId) {
        this.start();
      } else {
        this.stop();
      }
    });
  }

  ngOnDestroy(): void {
    this.stop();
    this.authSub?.unsubscribe();
  }


  extendSession(): void {

    this.warningShown = false;
  }


  forceLogout(): void {
    this.stop();
    this.authService.logout();
    this.router.navigate(['/home/auth/login']);
  }

  private start(): void {
    this.stop();
    this.warningShown = false;


    this.ngZone.runOutsideAngular(() => {
      this.checkSub = timer(0, this.CHECK_INTERVAL_MS).subscribe(() => {
        this.check();
      });
    });
  }

  private stop(): void {
    this.checkSub?.unsubscribe();
    this.checkSub = undefined;
    this.warningShown = false;
  }

  private check(): void {
    const token = this.authService.getToken();
    if (!token) return;

    let exp: number;
    try {
      const decoded: any = jwtDecode(token);
      exp = decoded.exp * 1000; // convert to ms
    } catch {
      return;
    }

    const now = Date.now();
    const remaining = exp - now;

    if (remaining <= 0) {

      this.ngZone.run(() => {
        this.stop();
        this.authService.logout();
        this.sessionExpired$.next();
      });
      return;
    }

    if (remaining <= this.WARNING_BEFORE_MS && !this.warningShown) {
      this.warningShown = true;
      const remainingSeconds = Math.floor(remaining / 1000);
      this.ngZone.run(() => {
        this.showWarning$.next({ remainingSeconds });
      });
    }
  }
}
