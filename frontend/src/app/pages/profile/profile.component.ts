import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ProfileService } from '../../core/services/profile-service/profile-service';
import { ProfileModel } from '../../shared/models/profile.model';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import {ItemModel} from '../../shared/models/item.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  standalone: false,
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  profile?: ProfileModel;
  isLoading: boolean = false;
  private destroy$ = new Subject<void>();
  items: ItemModel[] = [];

  constructor(
    private profileService: ProfileService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    if (!this.authService.isLoggedIn()) {
      console.log('User not authenticated, redirecting to login');
      this.router.navigate(['/login']);
      return;
    }

    this.loadProfile();
    this.loadMyItems();
  }

  loadProfile() {
    this.isLoading = true;

    this.profileService.getProfile()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.profile = data;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
  onEditItem(item: ItemModel) {
    // TODO
    console.log('Edit item:', item);
  }

  onViewItem(item: ItemModel) {
    // TODO
    console.log('View item:', item);
  }
  loadMyItems(): void {
    this.profileService.getMyItems()
      .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (data) => {
        this.items = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Greška pri učitavanju artikala', err);
      }
    });
  }
}
