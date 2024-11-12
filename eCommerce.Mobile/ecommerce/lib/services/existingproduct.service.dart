import 'dart:convert';
import 'package:ecommerce/models/colordetail.dart';
import 'package:ecommerce/models/productdetail.dart';
import 'package:ecommerce/models/productvariable.dart';
import 'package:ecommerce/models/sizedetail.dart';
import 'package:http/http.dart';

import 'package:ecommerce/utils/util.dart';


class ExistingProductService{

  List<String> productIds = [];
  List<ProductDetail> productDetails = [];

  bool productAdded = false;

  ExistingProductService();

  Future<void> getExistingProductIds(String userproductid) async{

    Map<String, dynamic> params = {
      "userproductid": userproductid,
    };

    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'ExistingProduct/GetProductIds',
      port: Util.port,
      queryParameters: params,
    );

    try{
      Response response = await get(uri);
      List<dynamic> data = jsonDecode(response.body);

      data.forEach((element){
        if (!productIds.contains(element))
        {
          productIds.add(element);
        }
      });
    }
    catch(err){
    }    
  }

  Future<void> getExistingProductDetails(String userproductid) async{

    Map<String, dynamic> params = {
      "userproductid": userproductid,
    };

    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'ExistingProduct/GetProducts',
      port: Util.port,
      queryParameters: params,
    );

    try{
      Response response = await get(uri);
      List<dynamic> data = jsonDecode(response.body);

      data.forEach((element){
        ProductDetail productDetail = ProductDetail();

        productDetail.userMobileNo = element['userMobileNo'] ?? 0;
        productDetail.addUpdate = element['addUpdate'] ?? '';
        productDetail.userProductId = element['userProductId'] ?? '';
        productDetail.title = element['title'] ?? '';
        productDetail.description = element['description'] ?? '';
        productDetail.categoryLinkId = element['categoryLinkId'] ?? 0;
        productDetail.subCategoryLinkId = element['subCategoryLinkId'] ?? 0;
        productDetail.isSelect = false;

        element['productVariables'].forEach((action){
          ProductVariable productVariable = ProductVariable();

          productVariable.index = action['index'];
          productVariable.price = action['price'];
          productVariable.discount = action['discount'];
          productVariable.inventory = action['inventory'];
          productVariable.imageShow = action['imageShow'];

          SizeDetail sizeDetail =  SizeDetail();
          sizeDetail.sizeId = action['sizeDetail']['sizeId'];
          sizeDetail.sizeLinkId = action['sizeDetail']['sizeLinkId'];
          sizeDetail.sizeCode = action['sizeDetail']['sizecode'];
          sizeDetail.description = action['sizeDetail']['description'];          
          productVariable.sizeDetail = sizeDetail;

          ColorDetail colorDetail =  ColorDetail();
          colorDetail.colorId = action['colorDetail']['colorId'];
          colorDetail.colorLinkId = action['colorDetail']['colorLinkId'];
          colorDetail.red = action['colorDetail']['red'];
          colorDetail.green = action['colorDetail']['green'];
          colorDetail.blue = action['colorDetail']['blue'];
          colorDetail.description = action['colorDetail']['description'];          
          productVariable.colorDetail = colorDetail;

          List<dynamic> imageUrls = action['imageUrls'];
          imageUrls.forEach((image){
            if (image.isNotEmpty)
            {
              productVariable.imageUrls.add(image);
            }
          });         

          productDetail.productVariables.add(productVariable);
        });

        productDetails.add(productDetail);
      });
    }
    catch(err){
    }       
  }

  Future<void> copyProducts(String mobileNo, List<String> toBeAddedProductIds, String toUserProductId) async {
    Map<String, dynamic> params = {
      "mobileNo": mobileNo,
      "toBeAddedProductIds": toBeAddedProductIds.toString().replaceAll('[', '').replaceAll(']', '').replaceAll(' ', ''),
      "toUserProductId": toUserProductId,
    };

    Uri uri = Uri(
      scheme: Util.filescheme,
      host: Util.filehost,
      path: 'ExistingProduct/CopyProducts',
      port: Util.fileport,
      queryParameters: params,
    );

    try{
      Response response = await get(uri);
      var data = jsonDecode(response.body);
      data['id'];
    }
    catch(err){}
  }

  Future<void> copyProduct(String mobileNo, String fromUserProductId, String toUserProductId) async {
    Map<String, dynamic> params = {
      "mobileNo": mobileNo,
      "fromUserProductId": fromUserProductId,
      "toUserProductId": toUserProductId,
    };

    Uri uri = Uri(
      scheme: Util.filescheme,
      host: Util.filehost,
      path: 'ExistingProduct/CopyProduct',
      port: Util.fileport,
      queryParameters: params,
    );

    try{
      Response response = await get(uri);
      var data = jsonDecode(response.body);
      if (data['id'] == 'success'){
        productAdded = true;
      }
    }
    catch(err){}
  }  
}