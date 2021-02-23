import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from 'src/environments/environment';

import { Gallery } from 'src/app/_models/gallery';

@Injectable({providedIn: 'root'})
export class HttpService {
  constructor(private http: HttpClient) { }

  private baseUrl: string = `${environment.apiSecureUrl}`;

  public getGalleries(): Observable<Array<Gallery>>
  {
    var url = this.baseUrl + 'api/gallery';

    return this.http.get<Array<Gallery>>(url);
  }
}
