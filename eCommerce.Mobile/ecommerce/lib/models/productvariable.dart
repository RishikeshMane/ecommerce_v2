
import 'package:ecommerce/models/sizedetail.dart';
import 'package:ecommerce/models/colordetail.dart';

class ProductVariable{
  ProductVariable();

  int index = 0;
  SizeDetail sizeDetail = SizeDetail();
  ColorDetail colorDetail = ColorDetail();
  List<String> imageUrls = [];
  int price = 0;
  int discount = 0;
  int inventory = 0;
  bool imageShow = false;
}