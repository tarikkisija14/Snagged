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
