import 'dart:convert';
import 'package:ecommerce/models/categorydetail.dart';
import 'package:ecommerce/models/colordetail.dart';
import 'package:ecommerce/models/productdetail.dart';
import 'package:ecommerce/models/productvariable.dart';
import 'package:ecommerce/models/sizedetail.dart';
import 'package:http/http.dart';

import 'package:ecommerce/utils/util.dart';

class ProductService{

  CategoryList categories = CategoryList();
  List<ProductVariable> productVariables = [];
  List<ProductDetail> productDetails = [];

  ProductService();

  Future<void> getCategories() async{

    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'Product/GetCategories',
      port: Util.port,
    );

    try{
      Response response = await get(uri);
      var data = jsonDecode(response.body);

      List<dynamic> category = data['category'];

      category.forEach((element){
        CategoryDetail categoryDetail = CategoryDetail();
        categoryDetail.category = element['category'];
        categoryDetail.categoryLinkId = element['categoryLinkId'];

        List<dynamic> subCategoryLinkIds = element['subCategoryLinkIds'];
        subCategoryLinkIds.forEach((action){
          categoryDetail.subCategoryLinkIds.add(action);
        });

        List<dynamic> subCategories = element['subCategories'];
        subCategories.forEach((action){
          categoryDetail.subCategories.add(action);
        });

        categories.category.add(categoryDetail);
      });      
    }
    catch(err){
    }    
  }

  Future<void> getProductVariable(String productId) async {

    Map<String, dynamic> params = {
      "productId": productId,
    };

    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'Product/GetProductVariable',
      port: Util.port,
      queryParameters: params,
    );

    try{
      Response response = await get(uri);
      var data = jsonDecode(response.body);

      List<dynamic> productvariabless = data;

      productvariabless.forEach((productvariable){

        ProductVariable variable = ProductVariable();
        variable.index = productvariable['index'];

        variable.sizeDetail.sizeId = productvariable['sizeDetail']['sizeId'];
        variable.sizeDetail.sizeLinkId = productvariable['sizeDetail']['sizeLinkId'];
        variable.sizeDetail.sizeCode = productvariable['sizeDetail']['sizecode'].toString();
        variable.sizeDetail.description = productvariable['sizeDetail']['description'];

        variable.colorDetail.colorId = productvariable['colorDetail']['colorId'];
        variable.colorDetail.colorLinkId = productvariable['colorDetail']['colorLinkId'];
        variable.colorDetail.red = productvariable['colorDetail']['red'];
        variable.colorDetail.green = productvariable['colorDetail']['green'];
        variable.colorDetail.blue = productvariable['colorDetail']['blue'];
        variable.colorDetail.description = productvariable['colorDetail']['description'];

        List<dynamic> imageUrls = productvariable['imageUrls'];
        imageUrls.forEach((image){
          if (image.isNotEmpty){
            variable.imageUrls.add(image);
          }
        });        
        
        variable.price = productvariable['price'];
        variable.discount = productvariable['discount'];
        variable.inventory = productvariable['inventory'];
        variable.imageShow = productvariable['imageShow'];

        productVariables.add(variable);
      });
    }
    catch(err){}
  }

  Future<void> getProductDetail(String userproductid) async{
    Map<String, dynamic> params = {
      "userproductid": userproductid,
    };

    Uri uri = Uri(
      scheme: Util.scheme,
      host: Util.host,
      path: 'Product/GetProducts',
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
    catch(err){}
  }

  Future<void> deleteProduct(String userproductid) async{
    Map<String, dynamic> params = {
      "userproductid": userproductid,
    };

    Uri uri = Uri(
      scheme: Util.filescheme,
      host: Util.filehost,
      path: 'Product/DeleteProduct',
      port: Util.fileport,
      queryParameters: params,
    );

    try{
      Response response = await delete(uri);
    }
    catch(err){}
  }
}