
import 'dart:core';

class CategoryList
{
    List<CategoryDetail> category=[];
}

class CategoryDetail{
  CategoryDetail();

  String category='';
  int categoryLinkId=0;
  List<int> subCategoryLinkIds=[];
  List<String> subCategories=[];
}
