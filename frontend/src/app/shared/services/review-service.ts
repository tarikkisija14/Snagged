
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Review } from '../models/review';

export interface AddReviewDto {
  reviewedUserId: number;
  rating: number;
  comment: string;
}

export interface UpdateReviewDto {
  rating: number;
  comment: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private readonly apiUrl = `${environment.apiUrl}/review`;

  constructor(private http: HttpClient) {}

  addReview(cmd: AddReviewDto): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.apiUrl}/add`, cmd);
  }

  getReviewById(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.apiUrl}/${id}`);
  }

  getReviewsByReviewedUser(reviewedUserId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/reviewed/${reviewedUserId}`);
  }

  getReviewsByReviewer(reviewerId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/reviewer/${reviewerId}`);
  }

  updateReview(id: number, cmd: UpdateReviewDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/update`, cmd);
  }

  deleteReview(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
