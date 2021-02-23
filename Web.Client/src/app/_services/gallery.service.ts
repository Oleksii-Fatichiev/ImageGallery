import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Gallery } from 'src/app/_models/gallery';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class GalleryService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = `${environment.apiSecureUrl}`;

  public getGalleries(): Observable<Array<Gallery>> {
    const url = this.baseUrl + 'api/gallery';
    return this.http.get<Array<Gallery>>(url);
  }

  public deleteGalleries(ids: number[]): Observable<any> {
    const url = this.baseUrl + 'api/gallery';
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }), body: ids };
    return this.http.delete<any>(url, httpOptions);
  }

  public createGallery(title: string, description: string): Observable<any> {
    const url = `${this.baseUrl}api/gallery`;
    return this.http.post<any>(url, {'name': title });
  }

  public updateGallery(id: number, title: string, description: string): Observable<any> {
    const url = `${this.baseUrl}api/gallery/${id}`;
    return this.http.put<any>(url, {'name': title });
  }
}
