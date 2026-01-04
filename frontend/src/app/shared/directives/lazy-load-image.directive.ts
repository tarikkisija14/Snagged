import {
  Directive,
  ElementRef,
  Input,
  Renderer2,
  OnInit,
  OnDestroy,
  HostBinding
} from '@angular/core';

@Directive({
  selector: 'img[appLazyLoad]',
  standalone: false
})
export class LazyLoadImageDirective implements OnInit, OnDestroy {
  @Input() appLazyLoad: string = '';
  @Input() placeholder: string = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="400" height="400"%3E%3Crect fill="%23f0f0f0" width="400" height="400"/%3E%3Ctext fill="%23999" x="50%25" y="50%25" text-anchor="middle" dy=".3em" font-family="sans-serif" font-size="18"%3ELoading...%3C/text%3E%3C/svg%3E';
  @Input() errorImage: string = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="400" height="400"%3E%3Crect fill="%23ffebee" width="400" height="400"/%3E%3Ctext fill="%23c62828" x="50%25" y="50%25" text-anchor="middle" dy=".3em" font-family="sans-serif" font-size="16"%3EImage Error%3C/text%3E%3C/svg%3E';
  @Input() rootMargin: string = '50px';

  @HostBinding('class.lazy-loading') isLoading = true;
  @HostBinding('class.lazy-loaded') isLoaded = false;
  @HostBinding('class.lazy-error') hasError = false;

  private intersectionObserver?: IntersectionObserver;

  constructor(
    private el: ElementRef<HTMLImageElement>,
    private renderer: Renderer2
  ) {}

  ngOnInit() {
    this.setImageSource(this.placeholder);
    this.setupIntersectionObserver();
  }

  private setupIntersectionObserver() {
    if (!('IntersectionObserver' in window)) {
      this.loadImage();
      return;
    }

    const options: IntersectionObserverInit = {
      root: null,
      rootMargin: this.rootMargin,
      threshold: 0.01
    };

    this.intersectionObserver = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          this.loadImage();
          this.intersectionObserver?.disconnect();
        }
      });
    }, options);

    this.intersectionObserver.observe(this.el.nativeElement);
  }

  private loadImage() {
    if (!this.appLazyLoad) {
      this.handleError();
      return;
    }

    const tempImage = new Image();

    tempImage.onload = () => {
      this.setImageSource(this.appLazyLoad);
      this.handleSuccess();
    };

    tempImage.onerror = () => {
      this.handleError();
    };

    tempImage.src = this.appLazyLoad;
  }

  private setImageSource(src: string) {
    this.renderer.setAttribute(this.el.nativeElement, 'src', src);
  }

  private handleSuccess() {
    this.isLoading = false;
    this.isLoaded = true;

    const img = this.el.nativeElement;
    this.renderer.setStyle(img, 'opacity', '0');
    this.renderer.setStyle(img, 'transition', 'opacity 0.3s ease-in');

    setTimeout(() => {
      this.renderer.setStyle(img, 'opacity', '1');
    }, 50);
  }

  private handleError() {
    this.isLoading = false;
    this.hasError = true;

    if (this.errorImage) {
      this.setImageSource(this.errorImage);
    }

    console.error('LazyLoadImageDirective: Failed to load image:', this.appLazyLoad);
  }

  ngOnDestroy() {
    if (this.intersectionObserver) {
      this.intersectionObserver.disconnect();
    }
  }
}
