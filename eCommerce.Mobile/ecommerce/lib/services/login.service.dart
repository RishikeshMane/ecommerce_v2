import 'dart:convert';
import 'package:http/http.dart';

import 'package:ecommerce/utils/util.dart';


class LoginService{

  LoginService();

  Future<dynamic> getLogin(String login, String password) async{

    Map<String, dynamic> params = {
      "number": login,
      "password": password,
    };

    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'User/GetLoginUrl',
      port: Util.port,
      queryParameters: params,
    );

    try{
      Response response = await get(uri);
      var data = jsonDecode(response.body);

      return data;

      ///return data["id"];
    }
    catch(err){
    }

    return 'false';     
  }
}