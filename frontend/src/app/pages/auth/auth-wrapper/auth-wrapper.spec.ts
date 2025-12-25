import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthWrapper } from './auth-wrapper';

describe('AuthWrapper', () => {
  let component: AuthWrapper;
  let fixture: ComponentFixture<AuthWrapper>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthWrapper]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthWrapper);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
