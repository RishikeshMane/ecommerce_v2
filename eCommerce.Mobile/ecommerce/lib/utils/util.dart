import 'dart:convert';

import 'package:ecommerce/models/categorydetail.dart';
import 'package:ecommerce/models/country.dart';
import 'package:ecommerce/models/productvariable.dart';
import 'package:ecommerce/models/state.dart';
import 'package:ecommerce/models/city.dart';
import 'package:localstorage/localstorage.dart';

class Util {

  static const String scheme = 'http';
  static const String host = '10.1.1.185';
  static const int port = 80;

  static const String filescheme = 'http';
  static const String filehost = '10.1.1.185';
  static const int fileport = 443;  

  static const String encryptedKey = '78526587238230947283626562564235232382983203023943499348734863677545346364563';

  static int getCountryLinkId(String country, List<Country> allCountries)
  {
    int countryLinkId = -1;

    allCountries.forEach((element){

      if (element.name == country)
      {
        countryLinkId = element.countryLinkId;
      }

    });      

    return countryLinkId;
  }

  static int getStateLinkId(String country, String state, List<Country> allCountries, List<States> allStates)
  {
    int stateLinkId = -1;
    int countryLinkId = getCountryLinkId(country, allCountries);

    allCountries.forEach((element){

      if (element.countryLinkId == countryLinkId)
      {
        allStates.forEach((action){

          if (action.name == state && action.countryLinkId == countryLinkId){
            stateLinkId = action.stateLinkId;
          }

        });
      }

    });     

    return stateLinkId;
  }

  static int getCityLinkId(String country, String state, String city, List<Country> allCountries, List<States> allStates, List<City> allCities)
  {
    int cityLinkId = -1;
    int stateLinkId = getStateLinkId(country, state, allCountries, allStates);

    allStates.forEach((action){

      if (action.name == state &&  action.stateLinkId == stateLinkId){
        
        allCities.forEach((action){

            if (action.name == city && action.stateLinkId == stateLinkId){
                cityLinkId = action.cityLinkId;
            }

        });
      }
    });    

    return cityLinkId;
  }     

  static List<String> getStates(String country, List<Country> allCountries, List<States> allStates)
  {
    List <String> states = [];

    int countryLinkId = getCountryLinkId(country, allCountries);

    allStates.forEach((action){

      if (action.countryLinkId == countryLinkId){
        states.add(action.name);
      }
    });

    return states;
  }

  static List<String> getCities(int stateLinkId, String country, List<Country> allCountries, List<City> allCities)
  {
    List <String> cities = [];

    int countryLinkId = getCountryLinkId(country, allCountries);

    allCities.forEach((action){
      if (action.stateLinkId == stateLinkId && action.countryLinkId == countryLinkId){
        cities.add(action.name);
      }
    });

    return cities;
  }

  static String getFlagCode(String countryName, List<Country> allCountries)
  {
    String flagCode = 'in';

    allCountries.forEach((country){
      if (country.name == countryName){
        flagCode = country.flagCode;
      }
    });

    return flagCode;
  }

  static String encrypt(String password){
    String encrypted='';

    for (int index = 0; index < password.length; index++) {

      String code = password.codeUnitAt(index).toString();

      if (code.length == 2)
      {
          code = '0$code';
      }           

      encrypted += (encryptedKey[index+index] + code + encryptedKey[index+index+1]);
    }

    decrypt(encrypted);

    return encrypted;    
  }

  static String decrypt(String password){
    String decrypted='';
    for (int index = 0; index < password.length; index+=5) {
        
        String code = password[index+1];
        code = '$code${password[index+2]}';
        code = '$code${password[index+3]}';

        decrypted = '$decrypted${String.fromCharCode(int.parse(code))}';
    }

    return decrypted;
  }

  static void updateUserDetail(dynamic response, String password)
  {
    deleteUserDetail();

    localStorage.setItem('id', response['id']);
    localStorage.setItem('role', response['role']);
    localStorage.setItem('userId', response['userId'].toString());
    localStorage.setItem('user', response['user']);
    localStorage.setItem('lastname', response['lastname']);
    localStorage.setItem('mobileno', response['mobileno'].toString());
    localStorage.setItem('pincode', response['pincode'].toString());
    ///localStorage.setItem('password', password);
  }

  static void deleteUserDetail(){
    localStorage.removeItem('id');
    localStorage.removeItem('role');
    localStorage.removeItem('userId');
    localStorage.removeItem('user');
    localStorage.removeItem('lastname');
    localStorage.removeItem('mobileno');
    localStorage.removeItem('pincode');
    localStorage.removeItem('password');
  }

  static Map<String, dynamic> getUserDetail(){
    Map<String, String> response = Map<String, String>();

    response['id'] = localStorage.getItem('id')!;
    response['role'] = localStorage.getItem('role')!;
    response['userId'] = localStorage.getItem('userId')!;
    response['user'] = localStorage.getItem('user')!;
    response['lastname'] = localStorage.getItem('lastname')!;
    response['mobileno'] = localStorage.getItem('mobileno')!;
    response['pincode'] = localStorage.getItem('pincode')!;
    ///response['password'] = localStorage.getItem('password')!;

    return response;
  }

  static String getUserProductId(){

    String mobileno = localStorage.getItem('mobileno')!;
    String pincode = localStorage.getItem('pincode')!;

    return '${mobileno[0]}${mobileno[1]}${pincode[4]}${pincode[5]}${mobileno[8]}${mobileno[9]}-';
  }

  static String getCategoryName(int categoryLinkId, CategoryList categories){
    String categoryName = '';

    categories.category.forEach((category){
      if (category.categoryLinkId == categoryLinkId){
          categoryName = category.category;
      }
    });

    return categoryName;
  }

  static String  getSubCategoryName(int categoryLinkId, int subCategoryLinkId, CategoryList categories){
    String subCategoryName = '';

    categories.category.forEach((category){
      if (category.categoryLinkId == categoryLinkId){
        int index = 0;

        category.subCategoryLinkIds.forEach((linkId){
            if (subCategoryLinkId == linkId){
                subCategoryName = category.subCategories[index];
            }
            ++index;
        });

      }
    });

    return subCategoryName;
  }

  static String getInventoryCount(List<ProductVariable> productVariables){
    int count = 0;

    productVariables.forEach((productVariable){
      count += productVariable.inventory;
    });

    return count.toString();
  }

  static List<List<String>> convertImageUrls(List<String> imageUrl){

    ///List<String> imageUrlss = ['A', 'B', 'C', 'D', 'E', 'F', 'G'];

    List<List<String>> imageUrls = [];

    for (int index=0; index < imageUrl.length; index = index+3){
      List<String> images = [];
      images.add(imageUrl[index]);
      if (index+1 < imageUrl.length)
      {
        images.add(imageUrl[index+1]);
      }

      if (index+2 < imageUrl.length)
      {
        images.add(imageUrl[index+2]);
      }      

      imageUrls.add(images);
    }

    return imageUrls;
  }
}

class ShowImage {
  static final ShowImage _showImage = ShowImage._getInstance();

  static bool _imageShow = false;
  static int _index = 0;
  
  factory ShowImage() {
    return _showImage;
  }

  void setShowImage(index, imageShow){
    _index = index;
    _imageShow = imageShow;
  }

  void resetShowImage(){
    _index = 0;
    _imageShow = false;
  }

  int getIndex(){
    return _index;
  }

  bool getImageShow(){
    return _imageShow;
  }    
  
  ShowImage._getInstance();
}