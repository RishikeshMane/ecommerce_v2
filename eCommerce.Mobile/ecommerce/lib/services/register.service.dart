import 'dart:convert';
import 'package:ecommerce/models/userdetail.dart';
import 'package:http/http.dart';

import 'package:ecommerce/utils/util.dart';
import 'package:ecommerce/models/country.dart';
import 'package:ecommerce/models/state.dart';
import 'package:ecommerce/models/city.dart';

class RegisterService{

  List<Country> allCountries = [];
  List<States> allStates = [];
  List<City> allCities = [];

  List<String> countries = [];
  List<String> states = [];
  List<String> cities = [];

  RegisterService();

  Future<void> getCountry() async{

      ///Uri ur = Uri.parse('http://worldtimeapi.org/api/timezone/$u');
      Uri uri = Uri(
        scheme: Util.scheme,
        host: Util.host,
        path: 'Country/GetCountry',
        port: Util.port,
      );
      Response response = await get(uri);
      ///Response response = await get(Uri.parse('http://worldtimeapi.org/api/timezone/Europe/London'));
      List<dynamic> data = jsonDecode(response.body);

      data.forEach((element){
        Country country = Country();

        country.countryId = element['countryId'];
        country.countryLinkId = element['countryLinkId'];
        country.name = element['name'];
        country.flagCode = element['flagCode'];

        allCountries.add(country);
        countries.add(country.name);
      });
  }

  Future<void> getState() async{
      Uri uri = Uri(
        scheme: Util.scheme,
        host: Util.host,
        path: 'Country/GetState',
        port: Util.port,
      );
      Response response = await get(uri);
      List<dynamic> data = jsonDecode(response.body);

      data.forEach((element){
        States state = States();

        state.stateId = element['stateId'];
        state.stateLinkId = element['stateLinkId'];
        state.countryLinkId = element['countryLinkId'];
        state.name = element['name'];

        allStates.add(state);
        states.add(state.name);
      });         
  }

  Future<void> getCity() async {
      Uri uri = Uri(
        scheme: Util.scheme,
        host: Util.host,
        path: 'Country/GetCity',
        port: Util.port,
      );
      Response response = await get(uri);

      if (response.statusCode == 200)
      {
        List<dynamic> data = jsonDecode(response.body);

        data.forEach((element){
          City city = City();

          city.cityId = element['cityId'];
          city.cityLinkId = element['cityLinkId'];
          city.stateLinkId = element['stateLinkId'];
          city.countryLinkId = element['countryLinkId'];
          city.name = element['name'];

          allCities.add(city);
          cities.add(city.name);
        });
      }                
  }

  Future<String> updateUsers(UserDetail user) async{
    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'User/UpsertUser',
      port: Util.port,
    );

    var body = jsonEncode(user.requestBody());

    try{

      Response response = await post(
                            uri,
                            headers: {"Content-Type": "application/json"},
                            body: body
                          );

      var data = jsonDecode(response.body);

      return data["id"];

      /**
      
      .then((response){

      })
      .then((error){

      });
      */

    }
    catch(err)
    {
      print(err);
    }

    return 'error';
  }   
}