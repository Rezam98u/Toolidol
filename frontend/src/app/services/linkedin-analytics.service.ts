import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LinkedInAnalyticsService {
	private readonly http = inject(HttpClient);
	private readonly base = '/api/linkedin-analytics';

	getPageViews(): Observable<any> {
		return this.http.get(`${this.base}/page-views`);
	}

	getFollowers(): Observable<any> {
		return this.http.get(`${this.base}/followers`);
	}

	getEngagement(): Observable<any> {
		return this.http.get(`${this.base}/engagement`);
	}
}
