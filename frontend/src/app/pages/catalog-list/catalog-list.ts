import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {Item} from '../../shared/models/item';
import {Category} from '../../shared/models/category';
import {Subcategory} from '../../shared/models/subcategory';
import {ItemService} from '../../shared/services/item-service';
import {CategoryService} from '../../shared/services/category-service';
import {SubcategoryService} from '../../shared/services/subcategory-service';
import {PageResult} from '../../shared/models/page-result';
import {environment} from '../../../environments/development';

@Component({
  selector: 'app-catalog-list',
  standalone: false,
  templateUrl: './catalog-list.html',
  styleUrl: './catalog-list.css',
})
export class CatalogList implements OnInit {

  items: Item[] = [];
  categories: Category[] = [];
  subcategories: Subcategory[] = [];
  allConditions: string[] = ['New', 'Excellent', 'Good', 'Fair'];


  selectedCategoryIds: number[] = [];
  selectedSubcategoryIds: number[] = [];
  selectedConditions: string[] = [];

  page: number = 1;
  pageSize: number = 14;
  totalItems: number = 0;

  minPrice: number = 0;
  maxPrice: number = 10000;
  currentMinPrice: number = 0;
  currentMaxPrice: number = 10000;

  selectedSortBy: string = 'newest';
  selectedSortOrder: string = 'desc';

  constructor(
    private itemService: ItemService,
    private categoryService: CategoryService,
    private subcategoryService: SubcategoryService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
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
      }
    });
  }

  loadItems(): void {
    console.log('=== LOADING ITEMS ===');


    const filter: any = {
      page: this.page,
      pageSize: this.pageSize,
      sortBy: this.selectedSortBy,
      sortOrder: this.selectedSortOrder
    };

    if (this.selectedCategoryIds.length > 0) {
      filter.categoryIds = this.selectedCategoryIds;
      console.log('Categories (OR within):', this.selectedCategoryIds);
    }


    if (this.selectedSubcategoryIds.length > 0) {
      filter.subcategoryIds = this.selectedSubcategoryIds;
      console.log('Subcategories (OR within):', this.selectedSubcategoryIds);
    }


    if (this.selectedConditions.length > 0) {
      filter.conditions = this.selectedConditions;
      console.log('Conditions (OR within):', this.selectedConditions);
    }


    if (this.currentMinPrice > this.minPrice) {
      filter.minPrice = this.currentMinPrice;
    }

    if (this.currentMaxPrice < this.maxPrice) {
      filter.maxPrice = this.currentMaxPrice;
    }

    console.log('Full filter object:', JSON.stringify(filter, null, 2));

    this.itemService.getFilteredItems(filter).subscribe({
      next: (res: PageResult<Item>) => {
        console.log('✅ Backend returned:', res.items?.length, 'items');
        console.log('Total in DB:', res.total);
        this.items = res.items || [];
        this.totalItems = res.total || 0;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('❌ Items error:', err);
        console.error('Error details:', err.status, err.message, err.error);
        this.items = [];
        this.cdr.detectChanges();
      }
    });
  }


  onCategoryChange(categoryId: number, checked: boolean): void {
    console.log('Category changed:', categoryId, checked);

    if (checked) {

      this.selectedCategoryIds = [categoryId];


      this.selectedSubcategoryIds = [];
      this.loadSubcategories();
    } else {
      this.selectedCategoryIds = [];
      this.subcategories = [];
      this.selectedSubcategoryIds = [];
    }

    this.loadItems();
  }






  onSubcategoryChange(subcategoryId: number, checked: boolean): void {
    console.log('Subcategory changed:', subcategoryId, checked);

    if (checked) {
      if (!this.selectedSubcategoryIds.includes(subcategoryId)) {
        this.selectedSubcategoryIds.push(subcategoryId);
      }
    } else {
      this.selectedSubcategoryIds = this.selectedSubcategoryIds.filter(id => id !== subcategoryId);
    }


    this.loadItems();
  }


  onConditionChange(condition: string, checked: boolean): void {
    console.log('Condition changed:', condition, checked);

    if (checked) {
      if (!this.selectedConditions.includes(condition)) {
        this.selectedConditions.push(condition);
      }
    } else {
      this.selectedConditions = this.selectedConditions.filter(c => c !== condition);
    }


    this.loadItems();
  }


  loadSubcategories(): void {
    if (this.selectedCategoryIds.length === 0) {
      this.subcategories = [];
      return;
    }

    console.log('Loading subcategories for categories:', this.selectedCategoryIds);


    this.subcategoryService.getSubcategories().subscribe({
      next: (allSubcategories) => {

        this.subcategories = allSubcategories.filter(sub =>
          sub.categoryId && this.selectedCategoryIds.includes(sub.categoryId)
        );
        console.log('✅ Filtered subcategories:', this.subcategories.length);
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('❌ Subcategories error:', err);
        this.subcategories = [];
        this.cdr.detectChanges();
      }
    });
  }

  getItemImage(item: Item): string {
    const baseUrl = 'https://localhost:7163';
    if (item.imageUrls && item.imageUrls.length > 0) {
      return `${baseUrl}${item.imageUrls[0]}`;
    }
    return `${baseUrl}/images/items/placeholder.png`;
  }

  clearFilters(): void {
    console.log('Clearing all filters...');

    this.selectedCategoryIds = [];
    this.selectedSubcategoryIds = [];
    this.selectedConditions = [];

    this.currentMinPrice = this.minPrice;
    this.currentMaxPrice = this.maxPrice;
    this.subcategories = [];

    this.loadItems();
  }


  applyFilters(): void {
    console.log('Applying filters...');
    this.loadItems();
  }

  onPriceInput(type: 'min' | 'max', event: any): void {
    const value = Number(event.target.value);

    if (type === 'min') {
      if (value > this.currentMaxPrice) {
        // Swap values
        const temp = this.currentMaxPrice;
        this.currentMaxPrice = value;
        this.currentMinPrice = temp;
      } else {
        this.currentMinPrice = value;
      }
    } else {
      if (value < this.currentMinPrice) {
        // Swap values
        const temp = this.currentMinPrice;
        this.currentMinPrice = value;
        this.currentMaxPrice = temp;
      } else {
        this.currentMaxPrice = value;
      }
    }


    this.cdr.detectChanges();


    setTimeout(() => {
      this.loadItems();
    }, 100);
  }


  onMinPriceChange(value: string): void {
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
    this.selectedConditions = [];
    this.loadItems();
  }


  clearPriceRange() {
    this.currentMinPrice = this.minPrice;
    this.currentMaxPrice = this.maxPrice;
    this.loadItems();
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




}
