import {
  Component, OnInit, OnDestroy, ChangeDetectorRef, ElementRef, HostListener,
} from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AuthService }         from '../../core/services/auth-service/AuthService';
import { Router }              from '@angular/router';
import { Subscription, interval } from 'rxjs';
import { switchMap, startWith } from 'rxjs/operators';
import { NotificationService } from '../../shared/services/notification-service';
import { Notification }        from '../../shared/models/notification';
import { SearchService }       from '../../shared/services/search-service';
import { SearchSuggestion }    from '../../shared/models/search-suggestion.model';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header implements OnInit, OnDestroy {
  isMobile = false;

  notifications: Notification[] = [];
  unreadCount    = 0;
  showNotifPanel = false;

  searchQuery      = '';
  searchOpen       = false;
  suggestions: SearchSuggestion[] = [];
  recentSearches: string[]        = [];
  searchLoading    = false;
  activeIndex      = -1;
  autocompleteHint = '';

  private breakpointSub?: Subscription;
  private authSub?:       Subscription;
  private pollSub?:       Subscription;
  private searchSub?:     Subscription;
  private loadingSub?:    Subscription;

  constructor(
    private breakpointObserver: BreakpointObserver,
    public  authService:         AuthService,
    private router:              Router,
    private notificationService: NotificationService,
    public  searchService:       SearchService,
    private cdr:                 ChangeDetectorRef,
    private elRef:               ElementRef,
  ) {}

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elRef.nativeElement.querySelector('.search-wrapper')?.contains(event.target)) {
      this.searchOpen      = false;
      this.activeIndex     = -1;
      this.autocompleteHint = '';
    }
  }

  ngOnInit(): void {
    this.breakpointSub = this.breakpointObserver
      .observe([Breakpoints.Handset, Breakpoints.Tablet])
      .subscribe(r => { this.isMobile = r.matches; });

    this.authSub = this.authService.currentUser$.subscribe(userId => {
      if (userId) { this.startPolling(); }
      else {
        this.stopPolling();
        this.notifications  = [];
        this.unreadCount    = 0;
        this.showNotifPanel = false;
      }
    });

    this.searchSub = this.searchService.suggestions.subscribe(s => {
      this.suggestions = s;
      this.activeIndex  = -1;
      this.updateHint();
      this.cdr.markForCheck();
    });

    this.loadingSub = this.searchService.loading.subscribe(l => {
      this.searchLoading = l;
      if (l) this.autocompleteHint = '';
      this.cdr.markForCheck();
    });
  }

  ngOnDestroy(): void {
    this.breakpointSub?.unsubscribe();
    this.authSub?.unsubscribe();
    this.searchSub?.unsubscribe();
    this.loadingSub?.unsubscribe();
    this.stopPolling();
  }

  private updateHint(): void {
    const q = this.searchQuery;
    if (!q || this.suggestions.length === 0) {
      this.autocompleteHint = '';
      return;
    }
    const first = this.suggestions[0].title;
    if (first.toLowerCase().startsWith(q.toLowerCase())) {
      this.autocompleteHint = q + first.slice(q.length);
    } else {
      this.autocompleteHint = '';
    }
  }

  onSearchInput(): void {
    this.autocompleteHint = '';
    this.searchService.search(this.searchQuery);
  }

  onSearchFocus(): void {
    this.searchOpen     = true;
    this.recentSearches = this.searchService.getRecentSearches();
    this.updateHint();
  }

  onSearchKeydown(event: KeyboardEvent): void {
    // Tab accepts the inline autocomplete hint
    if (event.key === 'Tab' && this.autocompleteHint && this.searchQuery) {
      event.preventDefault();
      this.searchQuery      = this.autocompleteHint;
      this.autocompleteHint = '';
      this.searchService.search(this.searchQuery);
      return;
    }

    // ArrowRight at end of input also accepts hint
    if (event.key === 'ArrowRight' && this.autocompleteHint && this.searchQuery) {
      const input = event.target as HTMLInputElement;
      if (input.selectionStart === input.value.length) {
        event.preventDefault();
        this.searchQuery      = this.autocompleteHint;
        this.autocompleteHint = '';
        this.searchService.search(this.searchQuery);
        return;
      }
    }

    const list  = this.searchQuery ? this.suggestions : this.recentSearches;
    const total = list.length;

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.activeIndex = Math.min(this.activeIndex + 1, total - 1);
        break;
      case 'ArrowUp':
        event.preventDefault();
        this.activeIndex = Math.max(this.activeIndex - 1, -1);
        break;
      case 'Enter':
        event.preventDefault();
        this.handleEnter();
        break;
      case 'Escape':
        this.searchOpen       = false;
        this.activeIndex      = -1;
        this.autocompleteHint = '';
        break;
    }
  }

  private handleEnter(): void {
    if (this.activeIndex >= 0 && this.searchQuery && this.suggestions[this.activeIndex]) {
      this.selectSuggestion(this.suggestions[this.activeIndex]);
    } else if (this.activeIndex >= 0 && !this.searchQuery && this.recentSearches[this.activeIndex]) {
      this.selectRecent(this.recentSearches[this.activeIndex]);
    } else if (this.searchQuery.trim()) {
      this.commitSearch(this.searchQuery.trim());
    }
  }

  selectSuggestion(s: SearchSuggestion): void {
    this.searchService.addRecentSearch(s.title);
    this.closeSearch();
    this.router.navigate(['/items', s.id]);
  }

  selectRecent(term: string): void {
    this.closeSearch();
    this.commitSearch(term);
  }

  deleteRecent(term: string): void {
    this.searchService.deleteRecentSearch(term);
    this.recentSearches = this.searchService.getRecentSearches();
  }

  clearRecent(): void {
    this.searchService.clearRecentSearches();
    this.recentSearches = [];
  }

  private commitSearch(term: string): void {
    const trimmed = term.trim();
    if (!trimmed) return;
    this.searchService.addRecentSearch(trimmed);
    this.searchService.clear();
    this.router.navigate(['/shop'], { queryParams: { q: trimmed } });
  }

  private closeSearch(): void {
    this.searchOpen       = false;
    this.activeIndex      = -1;
    this.searchQuery      = '';
    this.autocompleteHint = '';
    this.suggestions      = [];
    this.searchService.clear();
  }

  private startPolling(): void {
    this.stopPolling();
    this.pollSub = interval(30000).pipe(
      startWith(0),
      switchMap(() => this.notificationService.getMyNotifications())
    ).subscribe({
      next: list => {
        this.notifications = list;
        this.unreadCount   = list.filter(n => !n.isRead).length;
      },
      error: () => {}
    });
  }

  private stopPolling(): void {
    this.pollSub?.unsubscribe();
    this.pollSub = undefined;
  }

  toggleNotifPanel(): void { this.showNotifPanel = !this.showNotifPanel; }

  markAsRead(notif: Notification, event: Event): void {
    event.stopPropagation();
    if (notif.isRead) return;
    this.notificationService.markAsRead(notif.id).subscribe({
      next: () => { notif.isRead = true; this.unreadCount = Math.max(0, this.unreadCount - 1); },
      error: () => {}
    });
  }

  markAllRead(): void {
    this.notificationService.markAllAsRead().subscribe({
      next: () => { this.notifications.forEach(n => (n.isRead = true)); this.unreadCount = 0; },
      error: () => {}
    });
  }

  deleteNotification(notif: Notification, event: Event): void {
    event.stopPropagation();
    this.notificationService.deleteNotification(notif.id).subscribe({
      next: () => {
        this.notifications = this.notifications.filter(n => n.id !== notif.id);
        this.unreadCount   = this.notifications.filter(n => !n.isRead).length;
      },
      error: () => {}
    });
  }

  onUserIconClick(): void {
    if (this.authService.isLoggedIn()) { this.router.navigate(['/profile']); }
    else                               { this.router.navigate(['/home/auth/login']); }
  }

  logout(): void { this.authService.logout(); this.router.navigate(['/']); }
  goToCart(): void { this.router.navigate(['/cart']).catch(() => {}); }
}
