import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable, of, tap } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MainDataResolver implements Resolve<any> {
  constructor() {}
  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    console.log('Called in resolver...', route);
    return of({}).pipe(
      tap(() => console.warn('i will reach here and complete'))
    );
  }
}
