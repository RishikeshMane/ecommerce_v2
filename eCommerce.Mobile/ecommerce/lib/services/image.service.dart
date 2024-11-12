import 'dart:convert';
import 'package:http/http.dart';

import 'package:ecommerce/utils/util.dart';

class ImageService{

  ImageService();

  bool fileExist = false;

  Future<dynamic> fileExits(String fileUrl) async{

    /**
    Map<String, dynamic> params = {
      "number": login,
      "password": password,
    };
    */

    Uri uri = Uri(
      scheme: Util.filescheme,
      host: Util.filehost,
      path: fileUrl,
      port: Util.fileport,
      ///queryParameters: params,
    );

    try{
      Response response = await get(uri);
      if (response.reasonPhrase == 'OK'){
        fileExist = true;
      }
    }
    catch(err){
    }
  }
}