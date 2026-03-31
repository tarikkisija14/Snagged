import { Component, OnInit, OnDestroy } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { Router } from '@angular/router';
import { Subscription, interval } from 'rxjs';
import { switchMap, startWith } from 'rxjs/operators';
import { NotificationService } from '../../shared/services/notification-service';
import { Notification } from '../../shared/models/notification';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header implements OnInit, OnDestroy {
  isMobile = false;

  notifications: Notification[] = [];
  unreadCount = 0;
  showNotifPanel = false;

  private breakpointSub?: Subscription;
  private authSub?: Subscription;
  private pollSub?: Subscription;

  constructor(
    private breakpointObserver: BreakpointObserver,
    public authService: AuthService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.breakpointSub = this.breakpointObserver
      .observe([Breakpoints.Handset, Breakpoints.Tablet])
      .subscribe(result => {
        this.isMobile = result.matches;
      });

    if (this.authService.isLoggedIn()) {
      this.startPolling();
    }

    this.authSub = this.authService.currentUser$.subscribe(userId => {
      if (userId) {
        this.startPolling();
      } else {
        this.stopPolling();
        this.notifications = [];
        this.unreadCount = 0;
        this.showNotifPanel = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.breakpointSub?.unsubscribe();
    this.authSub?.unsubscribe();
    this.stopPolling();
  }

  private startPolling(): void {
    this.stopPolling();
    this.pollSub = interval(30000).pipe(
      startWith(0),
      switchMap(() => this.notificationService.getMyNotifications())
    ).subscribe({
      next: (list) => {
        this.notifications = list;
        this.unreadCount = list.filter(n => !n.isRead).length;
      },
      error: () => {}
    });
  }

  private stopPolling(): void {
    this.pollSub?.unsubscribe();
    this.pollSub = undefined;
  }

  toggleNotifPanel(): void {
    this.showNotifPanel = !this.showNotifPanel;
  }

  markAsRead(notif: Notification, event: Event): void {
    event.stopPropagation();
    if (notif.isRead) return;
    this.notificationService.markAsRead(notif.id).subscribe({
      next: () => {
        notif.isRead = true;
        this.unreadCount = Math.max(0, this.unreadCount - 1);
      },
      error: () => {}
    });
  }

  markAllRead(): void {
    this.notificationService.markAllAsRead().subscribe({
      next: () => {
        this.notifications.forEach(n => (n.isRead = true));
        this.unreadCount = 0;
      },
      error: () => {}
    });
  }

  deleteNotification(notif: Notification, event: Event): void {
    event.stopPropagation();
    this.notificationService.deleteNotification(notif.id).subscribe({
      next: () => {
        this.notifications = this.notifications.filter(n => n.id !== notif.id);
        this.unreadCount = this.notifications.filter(n => !n.isRead).length;
      },
      error: () => {}
    });
  }

  openLogin(): void {
    this.router.navigate(['home/auth/login']);
  }

  onUserIconClick() {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/profile']);
    } else {
      this.router.navigate(['/home/auth/login']);
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  goToCart() {
    this.router.navigate(['/cart']).catch(() => {});
  }
}
