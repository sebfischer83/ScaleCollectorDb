import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthModule } from '@auth0/auth0-angular';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AuthModule.forRoot({
      // The domain and clientId were configured in the previous chapter
      domain: 'dev-roekn3no.eu.auth0.com',
      clientId: 'MxG454pXZIm4U80TIJQojf8W86a2vqIk',

      // Request this audience at user authentication time
      audience: 'http://localhost:5000',

      // Request this scope at user authentication time
      scope: 'read:current_user',

      // Specify configuration for the interceptor
      httpInterceptor: {
        allowedList: [
          {
            // Match any request that starts 'https://dev-roekn3no.eu.auth0.com/api/v2/' (note the asterisk)
            uri: 'https://dev-roekn3no.eu.auth0.com/api/v2/*',
            tokenOptions: {
              // The attached token should target this audience
              audience: 'https://dev-roekn3no.eu.auth0.com/api/v2/',

              // The attached token should have these scopes
              scope: 'read:current_user',
            },
          },
          {
            uri: 'http://localhost:5000/*',
            allowAnonymous: true,
          },
          '/api/*',
        ],
      },
    }),
  ],
})
export class CoreModule {}
