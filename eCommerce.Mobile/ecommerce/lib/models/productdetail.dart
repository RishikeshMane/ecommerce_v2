
import 'package:ecommerce/models/categorydetail.dart';
import 'package:ecommerce/models/productvariable.dart';

class ProductDetail{
  ProductDetail();

  int userMobileNo = 0;
  String addUpdate = '';
  String userProductId = '';
  String title = '';
  String description = '';
  List<ProductVariable> productVariables = [];
  int categoryLinkId = 0;
  int subCategoryLinkId = 0;
  bool isSelect = false;
  CategoryList category = CategoryList();
}