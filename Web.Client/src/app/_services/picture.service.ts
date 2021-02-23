import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Gallery } from 'src/app/_models/gallery';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PictureService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = `${environment.apiSecureUrl}`;

  public getGalleries(): Observable<Array<Gallery>> {
    const url = this.baseUrl + 'api/picture';
    return this.http.get<Array<Gallery>>(url);
  }

  public deleteGalleries(ids: number[]): Observable<any> {
    const url = this.baseUrl + 'api/picture';
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }), body: ids };
    return this.http.delete<any>(url, httpOptions);
  }

  public createGallery(title: string, description: string): Observable<any> {
    const url = `${this.baseUrl}api/picture`;
    return this.http.post<any>(url, {'name': title });
  }

  public updateGallery(id: number, title: string, description: string): Observable<any> {
    const url = `${this.baseUrl}api/picture/${id}`;
    return this.http.put<any>(url, {'name': title });
  }




  public createPicture(picture: File): Observable<string> {
    const urlApi = `${environment.apiUrl}api/picture`;
    const formData = new FormData();
    formData.append('files', picture, picture.name);
    return this.http.post<string>(urlApi, formData);
  }
}
