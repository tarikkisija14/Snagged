import { Component, OnInit, ChangeDetectorRef, Input } from '@angular/core';
import { ItemModel } from '../../shared/models/item.model';
import { Category } from '../../shared/models/category';
import { Subcategory } from '../../shared/models/subcategory';
import { ItemService } from '../../shared/services/item-service';
import { CategoryService } from '../../shared/services/category-service';
import { SubcategoryService } from '../../shared/services/subcategory-service';
import { PageResult } from '../../shared/models/page-result';
import { CartService } from '../../shared/services/cart-service';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-catalog-list',
  standalone: false,
  templateUrl: './catalog-list.html',
  styleUrl: './catalog-list.scss',
})
export class CatalogList implements OnInit {
  items: ItemModel[] = [];
  categories: Category[] = [];
  subcategories: Subcategory[] = [];
  allConditions: string[] = ['New', 'Excellent', 'Good', 'Fair'];

  @Input() mode: 'home' | 'shop' = 'shop';

  selectedCategoryIds: number[] = [];
  selectedSubcategoryIds: number[] = [];
  selectedConditions: string[] = [];

  page     = 1;
  pageSize = 14;
  totalItems = 0;
  totalPages = 1;

  minPrice        = 0;
  maxPrice        = 10000;
  currentMinPrice = 0;
  currentMaxPrice = 10000;

  selectedSortBy    = 'newest';
  selectedSortOrder = 'desc';

  isLoading    = false;
  addedItemIds = new Set<number>();

  private readonly baseImageUrl = environment.apiUrl.replace('/api', '');

  constructor(
    private itemService: ItemService,
    private categoryService: CategoryService,
    private subcategoryService: SubcategoryService,
    private cdr: ChangeDetectorRef,
    private cartService: CartService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    if (this.mode === 'home') {
      this.pageSize = 9;
    }
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
        this.cdr.detectChanges();
        this.loadItems();
      },
      error: () => {
        this.loadItems();
      }
    });
  }

  loadItems(): void {
    this.isLoading = true;
    this.page      = Math.max(1, this.page);

    const filter: Record<string, unknown> = {
      'paging.page':     this.page,
      'paging.pageSize': this.pageSize,
      sortBy:            this.selectedSortBy,
      sortOrder:         this.selectedSortOrder,
    };

    if (this.selectedCategoryIds.length > 0)
      filter['categoryIds'] = this.selectedCategoryIds;

    if (this.selectedSubcategoryIds.length > 0)
      filter['subcategoryIds'] = this.selectedSubcategoryIds;

    if (this.selectedConditions.length > 0)
      filter['conditions'] = this.selectedConditions;

    if (this.currentMinPrice > this.minPrice)
      filter['minPrice'] = this.currentMinPrice;

    if (this.currentMaxPrice < this.maxPrice)
      filter['maxPrice'] = this.currentMaxPrice;

    this.itemService.getFilteredItems(filter).subscribe({
      next: (res: PageResult<ItemModel>) => {
        this.items      = res.items || [];
        this.totalItems = res.total || 0;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize) || 1;
        this.isLoading  = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.items      = [];
        this.totalItems = 0;
        this.totalPages = 1;
        this.isLoading  = false;
        this.cdr.detectChanges();
      }
    });
  }

  prevPage(): void {
    if (this.page > 1) { this.page--; this.loadItems(); }
  }

  nextPage(): void {
    if (this.page < this.totalPages) { this.page++; this.loadItems(); }
  }

  goToPage(pageNumber: number): void {
    if (pageNumber >= 1 && pageNumber <= this.totalPages) {
      this.page = pageNumber;
      this.loadItems();
    }
  }

  onCategoryChange(categoryId: number, checked: boolean): void {
    this.page = 1;
    if (checked) {
      this.selectedCategoryIds    = [categoryId];
      this.selectedSubcategoryIds = [];
      this.loadSubcategories();
    } else {
      this.selectedCategoryIds    = [];
      this.selectedSubcategoryIds = [];
      this.subcategories          = [];
      this.loadItems();
    }
  }

  onSubcategoryChange(subcategoryId: number, checked: boolean, categoryId: number): void {
    if (!this.selectedCategoryIds.includes(categoryId)) return;
    this.page = 1;

    if (checked) {
      if (!this.selectedSubcategoryIds.includes(subcategoryId))
        this.selectedSubcategoryIds = [...this.selectedSubcategoryIds, subcategoryId];
    } else {
      this.selectedSubcategoryIds = this.selectedSubcategoryIds.filter(id => id !== subcategoryId);
    }
    this.loadItems();
  }

  onConditionChange(condition: string, checked: boolean): void {
    this.page = 1;
    this.selectedConditions = checked
      ? [...this.selectedConditions.filter(c => c !== condition), condition]
      : this.selectedConditions.filter(c => c !== condition);
    this.loadItems();
  }

  loadSubcategories(): void {
    if (this.selectedCategoryIds.length === 0) {
      this.subcategories = [];
      this.loadItems();
      return;
    }

    const categoryId = this.selectedCategoryIds[0];

    this.subcategoryService.getSubcategories().subscribe({
      next: (all) => {
        this.subcategories = all.filter(sub => Number(sub.categoryId) === categoryId);
        this.cdr.detectChanges();
        this.loadItems();
      },
      error: () => {
        this.subcategories = [];
        this.loadItems();
      }
    });
  }

  onSortChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    const sortMap: Record<string, { sortBy: string; sortOrder: string }> = {
      'newest':         { sortBy: 'newest',     sortOrder: 'desc' },
      'price-low-high': { sortBy: 'price',      sortOrder: 'asc'  },
      'price-high-low': { sortBy: 'price',      sortOrder: 'desc' },
      'most-popular':   { sortBy: 'popularity', sortOrder: 'desc' },
    };
    const sort             = sortMap[value] ?? sortMap['newest'];
    this.selectedSortBy    = sort.sortBy;
    this.selectedSortOrder = sort.sortOrder;
    this.page = 1;
    this.loadItems();
  }

  onMinPriceChange(value: string): void {
    this.page = 1;
    const num = Number(value);
    this.currentMinPrice = isNaN(num)
      ? this.minPrice
      : Math.min(Math.max(num, this.minPrice), this.maxPrice);
    if (this.currentMinPrice > this.currentMaxPrice)
      this.currentMaxPrice = this.currentMinPrice;
    this.loadItems();
  }

  onMaxPriceChange(value: string): void {
    this.page = 1;
    const num = Number(value);
    this.currentMaxPrice = isNaN(num)
      ? this.maxPrice
      : Math.min(Math.max(num, this.minPrice), this.maxPrice);
    if (this.currentMaxPrice < this.currentMinPrice)
      this.currentMinPrice = this.currentMaxPrice;
    this.loadItems();
  }

  clearCondition(): void  { this.page = 1; this.selectedConditions = []; this.loadItems(); }
  clearPriceRange(): void {
    this.page = 1;
    this.currentMinPrice = this.minPrice;
    this.currentMaxPrice = this.maxPrice;
    this.loadItems();
  }

  clearFilters(): void {
    this.page                   = 1;
    this.selectedCategoryIds    = [];
    this.selectedSubcategoryIds = [];
    this.selectedConditions     = [];
    this.currentMinPrice        = this.minPrice;
    this.currentMaxPrice        = this.maxPrice;
    this.subcategories          = [];
    this.loadItems();
  }


  getItemImage(item: ItemModel): string {
    const mainImage = item.images?.find(img => img.isMain) ?? item.images?.[0];
    if (mainImage?.imageUrl) {
      return mainImage.imageUrl.startsWith('http')
        ? mainImage.imageUrl
        : `${this.baseImageUrl}${mainImage.imageUrl}`;
    }
    return `${this.baseImageUrl}/images/items/placeholder.png`;
  }

  addToCart(item: ItemModel): void {
    // SECURITY FIX: userId is no longer sent — the backend resolves it from the JWT token
    this.cartService.addCartItem(item.id, 1).subscribe({
      next: () => {
        this.addedItemIds = new Set(this.addedItemIds).add(item.id);
        this.cdr.detectChanges();
        setTimeout(() => {
          const updated = new Set(this.addedItemIds);
          updated.delete(item.id);
          this.addedItemIds = updated;
          this.cdr.detectChanges();
        }, 1000);
      },
      error: () => {
        const updated = new Set(this.addedItemIds);
        updated.delete(item.id);
        this.addedItemIds = updated;
        this.cdr.detectChanges();
      }
    });
  }
}
