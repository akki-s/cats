
##Technology Used
- dotnet Core 2.0
- angular js 
- XUnit for Unit Tests
- Moq for creating mocks in unit tests
- FluentAssertions for assertions in unit tests


##Assumptions
- When two owners of same gender have a cat with same name, then sort by owner name
- When there are no cats for a given gender, don't show that gender.
- When there are cats where owner gender is not specified, group them under empty string gender in Api. On the UI show it as Unspecified
- When there's no owner name, then show it as empty.
- When there's no cat name, then return it as empty in Api. In UI show it as 'a cat'.


##Details
- For quick view, i've deployed the app to Azure here -> [https://aakash-agl.azurewebsites.net/](https://aakash-agl.azurewebsites.net/)
- API endpoint to get grouped content -> [https://aakash-agl.azurewebsites.net/api/pets/cats](https://aakash-agl.azurewebsites.net/api/pets/cats)

## Angular JS Scaffold

Angular App was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.6.4.

