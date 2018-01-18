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
  ngOnInit(): void {
    this._httpService.get('/api/pets').subscribe(response => {
      console.log('api result', response.status);
      if (response.status == 200)
        this.petsResult = response.json();
      else
        this.message = response.statusText;
      console.log('GROUPED', this.petsResult);
    }); 
  }
}
