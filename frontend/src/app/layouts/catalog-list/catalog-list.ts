import {Component, OnInit, ChangeDetectorRef, Input} from '@angular/core';
import {Item} from '../../shared/models/item';
import {Category} from '../../shared/models/category';
import {Subcategory} from '../../shared/models/subcategory';
import {ItemService} from '../../shared/services/item-service';
import {CategoryService} from '../../shared/services/category-service';
import {SubcategoryService} from '../../shared/services/subcategory-service';
import {PageResult} from '../../shared/models/page-result';
import{CartService} from '../../shared/services/cart-service';
import{AuthService} from '../../core/services/auth-service/AuthService';

@Component({
  selector: 'app-catalog-list',
  standalone: false,
  templateUrl: './catalog-list.html',
  styleUrl: './catalog-list.scss',
})
export class CatalogList implements OnInit {

  items: Item[] = [];
  categories: Category[] = [];
  subcategories: Subcategory[] = [];
  allConditions: string[] = ['New', 'Excellent', 'Good', 'Fair'];

  @Input() mode: 'home' | 'shop' = 'shop';

  selectedCategoryIds: number[] = [];
  selectedSubcategoryIds: number[] = [];
  selectedConditions: string[] = [];


  page: number = 1;
  pageSize: number = 14;
  totalItems: number = 0;
  totalPages: number = 1;

  minPrice: number = 0;
  maxPrice: number = 10000;
  currentMinPrice: number = 0;
  currentMaxPrice: number = 10000;

  selectedSortBy: string = 'newest';
  selectedSortOrder: string = 'desc';

  isLoading: boolean = false;
  addedItemIds: Set<number> = new Set<number>();

  constructor(
    private itemService: ItemService,
    private categoryService: CategoryService,
    private subcategoryService: SubcategoryService,
    private cdr: ChangeDetectorRef,
    private cartService: CartService,
    private AuthService: AuthService
  ) { }

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
      error: (err) => {
        console.error('Categories error:', err);
        this.cdr.detectChanges();
        this.loadItems();
      }
    });
  }


  loadItems(): void {
    this.isLoading = true;
    this.page = Math.max(1, this.page);



    const filter: any = {
      page: 1,
      pageSize: (this.mode === 'home') ? 100 : 100,
      sortBy: this.selectedSortBy,
      sortOrder: this.selectedSortOrder,
      LoadAllItems:true
    };


    if (this.mode === 'shop') {
      filter.LoadAllItems = true;
    }


    if (this.selectedCategoryIds.length > 0) {
      filter.categoryIds = this.selectedCategoryIds;
    }

    if (this.selectedSubcategoryIds.length > 0) {
      filter.subcategoryIds = this.selectedSubcategoryIds;
    }

    if (this.selectedConditions.length > 0) {
      filter.conditions = this.selectedConditions;
    }

    if (this.currentMinPrice > this.minPrice) {
      filter.minPrice = this.currentMinPrice;
    }

    if (this.currentMaxPrice < this.maxPrice) {
      filter.maxPrice = this.currentMaxPrice;
    }


    this.itemService.getFilteredItems(filter).subscribe({
      next: (res: PageResult<Item>) => {

        this.items = (this.mode === 'home') ? res.items?.slice(0, 9) || [] : res.items || [];

        this.totalItems = res.total || 0;
        this.totalPages = 1;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Items error:', err);
        this.items = [];
        this.totalItems = 0;
        this.totalPages = 1;
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }


  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadItems();
    }
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadItems();
    }
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
      this.selectedCategoryIds = [categoryId];
      this.selectedSubcategoryIds = [];

      this.loadSubcategories();
    } else {
      this.selectedCategoryIds = [];
      this.selectedSubcategoryIds = [];
      this.subcategories = [];

      this.loadItems();
    }
  }

  onSubcategoryChange(subcategoryId: number, checked: boolean, categoryId: number): void {

    this.page = 1;

    if (!this.selectedCategoryIds.includes(categoryId)) return;

    let newSubcategoryIds = [...this.selectedSubcategoryIds];

    if (checked) {
      if (!newSubcategoryIds.includes(subcategoryId)) {
        newSubcategoryIds.push(subcategoryId);
      }
    } else {
      newSubcategoryIds = newSubcategoryIds.filter(id => id !== subcategoryId);
    }
    this.selectedSubcategoryIds = newSubcategoryIds;

    this.loadItems();
  }

  onConditionChange(condition: string, checked: boolean): void {

    this.page = 1;

    let newConditions = [...this.selectedConditions];

    if (checked) {
      if (!newConditions.includes(condition)) {
        newConditions.push(condition);
      }
    } else {
      newConditions = newConditions.filter(c => c !== condition);
    }
    this.selectedConditions = newConditions;

    this.loadItems();
  }

  loadSubcategories(): void {

    if (this.selectedCategoryIds.length === 0) {
      this.subcategories = [];
      this.loadItems();
      return;
    }

    const categoryIdToFilter = this.selectedCategoryIds[0];

    this.subcategoryService.getSubcategories().subscribe({
      next: (allSubcategories) => {
        this.subcategories = allSubcategories.filter(sub =>
          Number(sub.categoryId) === categoryIdToFilter
        );

        this.cdr.detectChanges();

        this.loadItems();
      },
      error: (err) => {
        console.error(' Subcategories error:', err);
        this.subcategories = [];
        this.cdr.detectChanges();
        this.loadItems();
      }
    });
  }

  onSortChange(event: any): void {

    const value = event.target.value;

    switch (value) {
      case 'newest':
        this.selectedSortBy = 'newest';
        this.selectedSortOrder = 'desc';
        break;
      case 'price-low-high':
        this.selectedSortBy = 'price';
        this.selectedSortOrder = 'asc';
        break;
      case 'price-high-low':
        this.selectedSortBy = 'price';
        this.selectedSortOrder = 'desc';
        break;
      case 'most-popular':
        this.selectedSortBy = 'popularity';
        this.selectedSortOrder = 'desc';
        break;
    }

    this.page = 1;
    this.loadItems();
  }


  onMinPriceChange(value: string): void {
    this.page = 1;
    const numValue = Number(value);

    if (isNaN(numValue) || numValue < this.minPrice) {
      this.currentMinPrice = this.minPrice;
    } else if (numValue > this.maxPrice) {
      this.currentMinPrice = this.maxPrice;
    } else {
      this.currentMinPrice = numValue;
    }

    if (this.currentMinPrice > this.currentMaxPrice) {
      this.currentMaxPrice = this.currentMinPrice;
    }

    this.loadItems();
  }

  onMaxPriceChange(value: string): void {
    this.page = 1;
    const numValue = Number(value);

    if (isNaN(numValue) || numValue < this.minPrice) {
      this.currentMaxPrice = this.minPrice;
    } else if (numValue > this.maxPrice) {
      this.currentMaxPrice = this.maxPrice;
    } else {
      this.currentMaxPrice = numValue;
    }

    if (this.currentMaxPrice < this.currentMinPrice) {
      this.currentMinPrice = this.currentMaxPrice;
    }

    this.loadItems();
  }

  clearCondition(): void {
    this.page = 1;
    this.selectedConditions = [];
    this.loadItems();
  }

  clearPriceRange() {
    this.page = 1;
    this.currentMinPrice = this.minPrice;
    this.currentMaxPrice = this.maxPrice;
    this.loadItems();
  }

  clearFilters(): void {
    this.page = 1;
    this.selectedCategoryIds = [];
    this.selectedSubcategoryIds = [];
    this.selectedConditions = [];
    this.currentMinPrice = this.minPrice;
    this.currentMaxPrice = this.maxPrice;
    this.subcategories = [];
    this.loadItems();
  }

  getItemImage(item: Item): string {
    const baseUrl = 'https://localhost:7163';
    if (item.imageUrls && item.imageUrls.length > 0) {
      return `${baseUrl}${item.imageUrls[0]}`;
    }
    return `${baseUrl}/images/items/placeholder.png`;
  }

  addToCart(item: Item) {
    const quantity = 1;

    this.cartService.addCartItem(item.id, quantity).subscribe({
      next: () => {

        this.addedItemIds = new Set(this.addedItemIds).add(item.id);
        this.cdr.detectChanges();

        setTimeout(() => {
          const newSet = new Set(this.addedItemIds);
          newSet.delete(item.id);
          this.addedItemIds = newSet;
          this.cdr.detectChanges();
        }, 1000);
      },
      error: (err) => {
        console.error('Error adding item to cart:', err);
        const newSet = new Set(this.addedItemIds);
        newSet.delete(item.id);
        this.addedItemIds = newSet;
        this.cdr.detectChanges();
      }
    });
  }

}
