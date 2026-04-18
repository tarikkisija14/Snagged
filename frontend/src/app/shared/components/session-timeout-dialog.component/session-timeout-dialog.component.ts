import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
} from '@angular/core';
import { Subscription, interval } from 'rxjs';
import {SessionTimeoutService} from '../../../core/services/session-timeout-service/session-timeout.service';

@Component({
  selector: 'app-session-timeout-dialog',
  standalone: false,
  templateUrl: './session-timeout-dialog.component.html',
  styleUrls: ['./session-timeout-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SessionTimeoutDialogComponent implements OnInit, OnDestroy {

  isVisible = false;
  remainingSeconds = 120;

  private warningSub?: Subscription;
  private expiredSub?: Subscription;
  private countdownSub?: Subscription;

  constructor(
    private sessionTimeoutService: SessionTimeoutService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.warningSub = this.sessionTimeoutService.showWarning$.subscribe(event => {
      this.remainingSeconds = event.remainingSeconds;
      this.isVisible = true;
      this.startCountdown();
      this.cdr.markForCheck();
    });

    this.expiredSub = this.sessionTimeoutService.sessionExpired$.subscribe(() => {
      this.isVisible = false;
      this.stopCountdown();
      this.cdr.markForCheck();
    });
  }

  ngOnDestroy(): void {
    this.warningSub?.unsubscribe();
    this.expiredSub?.unsubscribe();
    this.stopCountdown();
  }

  get formattedTime(): string {
    const m = Math.floor(this.remainingSeconds / 60);
    const s = this.remainingSeconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
  }

  get progressPercent(): number {
    return Math.max(0, (this.remainingSeconds / 120) * 100);
  }

  onExtend(): void {
    this.isVisible = false;
    this.stopCountdown();
    this.sessionTimeoutService.extendSession();
    this.cdr.markForCheck();
  }

  onLogout(): void {
    this.isVisible = false;
    this.stopCountdown();
    this.sessionTimeoutService.forceLogout();
    this.cdr.markForCheck();
  }

  private startCountdown(): void {
    this.stopCountdown();
    this.countdownSub = interval(1000).subscribe(() => {
      this.remainingSeconds = Math.max(0, this.remainingSeconds - 1);
      if (this.remainingSeconds === 0) {
        this.stopCountdown();
        this.isVisible = false;
      }
      this.cdr.markForCheck();
    });
  }

  private stopCountdown(): void {
    this.countdownSub?.unsubscribe();
    this.countdownSub = undefined;
  }
}
