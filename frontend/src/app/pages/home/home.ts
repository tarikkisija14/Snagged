import { Component, inject } from '@angular/core';
import {Router} from '@angular/router';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {map, Observable, shareReplay} from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  private breakpointObserver = inject(BreakpointObserver);

  constructor(private router: Router) {}

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map(result=> result.matches),
      shareReplay()
    );

  goToShop() {
    this.router.navigate(['/shop']);
  }
}
