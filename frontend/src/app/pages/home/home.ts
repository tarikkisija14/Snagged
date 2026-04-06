import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, Observable, shareReplay, Subscription, filter } from 'rxjs';
import Swiper from 'swiper';
import { Pagination, Autoplay, Navigation } from 'swiper/modules';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit, OnDestroy {
  private breakpointObserver = inject(BreakpointObserver);
  private swiper?: Swiper;
  private routerSubscription?: Subscription;
  private initTimer?: ReturnType<typeof setTimeout>;

  constructor(private router: Router) {}

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  ngOnInit(): void {
    this.initTimer = setTimeout(() => {
      this.initSwiper();

      this.routerSubscription = this.router.events
        .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
        .subscribe((event: NavigationEnd) => {
          if (this.swiper?.autoplay) {
            if (event.url === '/' || event.url === '/home') {
              this.swiper.autoplay.start();
            } else {
              this.swiper.autoplay.stop();
            }
          }
        });
    }, 100);
  }

  ngOnDestroy(): void {
    if (this.initTimer !== undefined) {
      clearTimeout(this.initTimer);
    }
    this.routerSubscription?.unsubscribe();
    if (this.swiper) {
      this.swiper.destroy(true, true);
    }
  }

  private initSwiper(): void {
    this.swiper = new Swiper('.hero-swiper', {
      modules: [Pagination, Autoplay, Navigation],
      effect: 'slide',
      pagination: {
        el: '.swiper-pagination',
        clickable: true,
      },
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      loop: true,
      autoplay: {
        delay: 3500,
        disableOnInteraction: false,
        pauseOnMouseEnter: true,
      },
      speed: 600,
      allowTouchMove: false,
      keyboard: {
        enabled: true,
      },
    });
  }

  goToShop(): void {
    this.router.navigate(['/shop']);
  }
}
