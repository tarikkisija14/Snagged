import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ProfileService }  from '../../shared/services/profile-service';
import { ProfileModel }    from '../../shared/models/profile.model';
import { AuthService }     from '../../core/services/auth-service/AuthService';
import { Router }          from '@angular/router';
import { Subject }         from 'rxjs';
import { takeUntil }       from 'rxjs/operators';
import { ItemModel }       from '../../shared/models/item.model';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  profile?: ProfileModel;
  isLoading    = false;
  profileError = false;
  items: ItemModel[] = [];

  // FIX: Inline SVG data URI used as fallback — no external file required.
  readonly defaultAvatar =
    'data:image/svg+xml,%3Csvg xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22 viewBox%3D%220 0 40 40%22%3E%3Ccircle cx%3D%2220%22 cy%3D%2220%22 r%3D%2220%22 fill%3D%22%23e0e0e0%22%2F%3E%3Ccircle cx%3D%2220%22 cy%3D%2215%22 r%3D%226%22 fill%3D%22%23bdbdbd%22%2F%3E%3Cellipse cx%3D%2220%22 cy%3D%2232%22 rx%3D%2210%22 ry%3D%228%22 fill%3D%22%23bdbdbd%22%2F%3E%3C%2Fsvg%3E';

  private destroy$ = new Subject<void>();

  constructor(
    private profileService: ProfileService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/home/auth/login']);
      return;
    }
    this.loadProfile();
    this.loadMyItems();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadProfile(): void {
    this.isLoading    = true;
    this.profileError = false;

    this.profileService.getProfile()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.profile   = data;
          this.isLoading = false;
          this.cdr.markForCheck();
        },
        error: () => {
          this.isLoading    = false;
          this.profileError = true;
          this.cdr.markForCheck();
        },
      });
  }

  loadMyItems(): void {
    this.profileService.getMyItems()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next:  (data) => { this.items = data; this.cdr.markForCheck(); },
        error: () => {},
      });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  onViewItem(item: ItemModel): void {
    this.router.navigate(['/items', item.id]);
  }

  onEditItem(_item: ItemModel): void {
    // TODO: implement item editing
  }
}
