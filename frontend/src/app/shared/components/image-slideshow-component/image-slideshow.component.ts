import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  HostListener,
} from '@angular/core';
import { ItemImageModel } from '../../models/item-image.model';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-image-slideshow',
  standalone: false,
  templateUrl: './image-slideshow.component.html',
  styleUrls: ['./image-slideshow.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImageSlideshowComponent implements OnChanges, OnDestroy {
  @Input() images: ItemImageModel[] = [];
  @Input() itemTitle = '';
  @Input() isSold    = false;
  @Input() enableZoom = true;

  activeIndex    = 0;
  isTransitioning = false;

  private readonly baseUrl      = environment.apiUrl.replace('/api', '');
  private readonly placeholder  = `/images/items/placeholder.png`;


  private touchStartX = 0;
  private touchStartY = 0;

  constructor(private cdr: ChangeDetectorRef) {}



  ngOnChanges(changes: SimpleChanges): void {
    if (changes['images']) {
      const mainIdx = this.images.findIndex(img => img.isMain);
      this.activeIndex = mainIdx >= 0 ? mainIdx : 0;
      this.cdr.markForCheck();
    }
  }

  ngOnDestroy(): void {}



  get activeImage(): ItemImageModel | null {
    return this.images[this.activeIndex] ?? null;
  }

  get activeImageUrl(): string {
    return this.resolveUrl(this.activeImage?.imageUrl);
  }

  get hasMultiple(): boolean {
    return this.images.length > 1;
  }



  selectIndex(index: number): void {
    if (index === this.activeIndex || this.isTransitioning) return;
    this.isTransitioning = true;
    this.activeIndex = index;
    this.cdr.markForCheck();
    setTimeout(() => {
      this.isTransitioning = false;
      this.cdr.markForCheck();
    }, 300);
  }

  prev(): void {
    const next = (this.activeIndex - 1 + this.images.length) % this.images.length;
    this.selectIndex(next);
  }

  next(): void {
    const next = (this.activeIndex + 1) % this.images.length;
    this.selectIndex(next);
  }


  onTouchStart(event: TouchEvent): void {
    this.touchStartX = event.touches[0].clientX;
    this.touchStartY = event.touches[0].clientY;
  }

  onTouchEnd(event: TouchEvent): void {
    if (!this.hasMultiple) return;
    const dx = event.changedTouches[0].clientX - this.touchStartX;
    const dy = event.changedTouches[0].clientY - this.touchStartY;
    if (Math.abs(dx) > Math.abs(dy) && Math.abs(dx) > 40) {
      dx < 0 ? this.next() : this.prev();
    }
  }



  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (!this.hasMultiple) return;
    if (event.key === 'ArrowLeft')  { this.prev(); event.preventDefault(); }
    if (event.key === 'ArrowRight') { this.next(); event.preventDefault(); }
  }



  resolveUrl(imageUrl: string | undefined | null): string {
    if (!imageUrl) return `${this.baseUrl}${this.placeholder}`;
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl;
    return `${this.baseUrl}${imageUrl}`;
  }

  onMainImageError(event: Event): void {
    (event.target as HTMLImageElement).src = `${this.baseUrl}${this.placeholder}`;
  }

  onThumbError(event: Event): void {
    (event.target as HTMLImageElement).src = `${this.baseUrl}${this.placeholder}`;
  }

  trackById(_: number, img: ItemImageModel): number {
    return img.id;
  }
}
