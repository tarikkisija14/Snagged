import {
  Directive,
  ElementRef,
  Input,
  OnDestroy,
  Renderer2,
  HostListener,
} from '@angular/core';

@Directive({
  selector: '[appImageZoom]',
  standalone: false,
})
export class ImageZoomDirective implements OnDestroy {
  @Input() appImageZoom = '';
  @Input() zoomFactor   = 2;

  private zoomed       = false;
  private originX      = 50;
  private originY      = 50;

  constructor(
    private el: ElementRef<HTMLElement>,
    private renderer: Renderer2,
  ) {}

  ngOnDestroy(): void {
    this.resetZoom();
  }

  @HostListener('click', ['$event'])
  onClick(event: MouseEvent): void {
    if (!this.appImageZoom) return;

    if (this.zoomed) {
      this.resetZoom();
      return;
    }

    const host  = this.el.nativeElement;
    const rect  = host.getBoundingClientRect();
    const img   = host.querySelector('img') as HTMLImageElement;
    if (!img) return;

    // Calculate click position as percentage within the element
    const x = ((event.clientX - rect.left) / rect.width)  * 100;
    const y = ((event.clientY - rect.top)  / rect.height) * 100;

    this.originX = x;
    this.originY = y;
    this.zoomed  = true;

    this.renderer.setStyle(img, 'transform',        `scale(${this.zoomFactor})`);
    this.renderer.setStyle(img, 'transform-origin', `${x}% ${y}%`);
    this.renderer.setStyle(img, 'transition',       'transform 0.3s ease');
    this.renderer.setStyle(img, 'cursor',           'zoom-out');
    this.renderer.setStyle(host, 'cursor',          'zoom-out');
  }

  @HostListener('mousemove', ['$event'])
  onMouseMove(event: MouseEvent): void {
    if (!this.zoomed) return;

    const host = this.el.nativeElement;
    const rect = host.getBoundingClientRect();
    const img  = host.querySelector('img') as HTMLImageElement;
    if (!img) return;

    // Smoothly pan the zoom origin as mouse moves
    const x = ((event.clientX - rect.left) / rect.width)  * 100;
    const y = ((event.clientY - rect.top)  / rect.height) * 100;

    this.renderer.setStyle(img, 'transform-origin', `${x}% ${y}%`);
  }

  @HostListener('mouseleave')
  onMouseLeave(): void {
    if (this.zoomed) {
      this.resetZoom();
    }
  }

  private resetZoom(): void {
    const host = this.el.nativeElement;
    const img  = host.querySelector('img') as HTMLImageElement;
    if (img) {
      this.renderer.setStyle(img, 'transform',        'scale(1)');
      this.renderer.setStyle(img, 'transform-origin', 'center center');
      this.renderer.setStyle(img, 'transition',       'transform 0.3s ease');
      this.renderer.setStyle(img, 'cursor',           'zoom-in');
    }
    this.renderer.setStyle(host, 'cursor', 'zoom-in');
    this.zoomed = false;
  }
}
