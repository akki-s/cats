import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { PetsGrouped } from './models/pets.grouped';  

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private _httpService: Http) { }
  title: string = 'AGL Coding Test';
  petsResult: PetsGrouped[] = [];
  message: string = '';
  isLoading: boolean = true;
  ngOnInit(): void {
    this._httpService.get('/api/pets').subscribe(response => {
      console.log('api result', response.status, response.statusText);
      if (response.status == 200)
      {
        this.petsResult = response.json();
        if (this.petsResult.length == 0)
          this.message = 'No data returned from Api';
      }
      else
        this.message = response.statusText + ' - There is an issue with Api.';
      console.log('GROUPED', this.petsResult);
      this.isLoading = false;
    }); 
  }
}
