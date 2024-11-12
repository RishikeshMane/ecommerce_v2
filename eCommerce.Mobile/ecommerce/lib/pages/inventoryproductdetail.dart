import 'package:flutter/material.dart';

import 'package:ecommerce/pages/subpages/inventoryproductdetailvariable.dart';
import 'package:ecommerce/services/product.service.dart';
import 'package:ecommerce/models/productdetail.dart';
import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/utils/util.dart';

class InventoryProductDetailPage extends StatefulWidget {
  const InventoryProductDetailPage({super.key, required this.title});

  final String title;

  @override
  State<InventoryProductDetailPage> createState() => InventoryProductDetailPageState();
}

class InventoryProductDetailPageState extends State<InventoryProductDetailPage> {

  ProductDetail _productDetail = ProductDetail();
  String _category = '';
  String _subCategory = '';

  String _productDetailTitle = '';
  String _productDetailDescription = '';

  @override
  void initState() {
    updateProductVariable();
    super.initState();
  }

  Future<void> updateProductVariable() async {

    ProductService ps = ProductService();
    if (_productDetail.userProductId.isEmpty) return;
    await ps.getProductVariable(_productDetail.userProductId);
    try{
      setState((){
        _productDetail.productVariables.clear();
        _productDetail.productVariables = ps.productVariables;
        updateShowImages();
      }
      );
    }
    catch(err){}
  }

  void updateShowImages(){
    _productDetail.productVariables.forEach((productVariable){
      if (productVariable.index == ShowImage().getIndex()){
        productVariable.imageShow = ShowImage().getImageShow();
      }
    });
  }

  void updateCategory(){
    _category = Util.getCategoryName(_productDetail.categoryLinkId, _productDetail.category);
  }

  void updateSubCategory(){
    _subCategory = Util.getSubCategoryName(_productDetail.categoryLinkId, _productDetail.subCategoryLinkId, _productDetail.category);
  }

  void updateTextField(){
    _productDetailTitle = _productDetail.title;
    _productDetailDescription = _productDetail.description;
  }

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {

    _productDetail = ModalRoute.of(context)!.settings.arguments as ProductDetail;
    updateCategory();
    updateSubCategory();
    updateProductVariable();

    updateTextField();

    return Scaffold(
      resizeToAvoidBottomInset: true,
      backgroundColor: Style.pageBackground,
      appBar: AppBar(
        title: Text(
          ///widget.title,
          _productDetail.userProductId,
          style: const TextStyle(
            color: Color.fromRGBO(245, 245, 220, 1.0),
            fontSize: 21,
          ),
          ),
        actions: <Widget>[  
          IconButton(
            icon: const Icon(
              Icons.logout,
              color: Colors.white,
              size: 35,
            ),
            onPressed: () {
              Util.deleteUserDetail();
              Navigator.pushReplacementNamed(context, '/login');
            },
          )
        ],          
        centerTitle: true,
        elevation: 0.0,
        backgroundColor: const Color.fromRGBO(51, 155, 124, 1.0),
      ),
      body: SingleChildScrollView(
        child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              const SizedBox(
                height: 12,
              ),

              const Center(
                child: CircleAvatar(
                  backgroundColor: Color.fromRGBO(51, 155, 124, 1.0),
                  radius: 27.0,
                  child: CircleAvatar(
                    backgroundImage: AssetImage('assets/images/bagwanism.jpg'),
                    radius: 25.0,
                  ),
                ),
              ),

              const SizedBox(
                height: 21,
              ),

              const Center(
                child: Text(
                  'Title',
                  style: TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: 16,
                    color: Color.fromRGBO(21, 21, 21, 1.0),
                  ),
                ),
              ),
              const SizedBox(
                height: 2,
              ),
              Center(
                child: TextField(
                  decoration: InputDecoration(
                    filled: true,
                    fillColor: Style.textBackground,
                    labelText: _productDetailTitle,
                    labelStyle: const TextStyle(
                      color: Color.fromRGBO(21, 21, 21, 1.0),
                    ),             
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10.0),
                    ),
                  ),
                  style: const TextStyle(
                    height: 1.0,
                  ),

                  onChanged: (String value){
                                setState(() {
                                  _productDetailTitle = _productDetail.title;                
                              });},   
                  ),
              ),
              const Divider(),

              const Center(
                child: Text(
                  'Description',
                  style: TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: 16,
                    color: Color.fromRGBO(21, 21, 21, 1.0),
                  ),                 
                ),
              ),
              const SizedBox(
                height: 2,
              ),
              Center(
                child: TextFormField(
                  minLines: 3,
                  maxLines: 3,
                  decoration: InputDecoration(
                    filled: true,
                    fillColor: Style.textBackground,
                    labelText: _productDetailDescription,
                    labelStyle: const TextStyle(
                      color: Color.fromRGBO(21, 21, 21, 1.0),
                    ),                                 
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10.0),
                    ),
                  ),
                  style: const TextStyle(
                    height: 1.0,
                  ),

                  onChanged: (String value){
                                setState(() {
                                  _productDetailDescription = _productDetail.description;                
                              });},   
                  ),
              ),
              const Divider(),
              const SizedBox(
                height: 11,
              ),

              Row(
                mainAxisAlignment: MainAxisAlignment.center,

                children: <Widget>[
                  Expanded(
                    flex: 15,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.start,
                      children: <Widget>[
                        const Text(
                          '  Category  ',
                          style: TextStyle(
                                  fontWeight: FontWeight.w500,
                                  fontSize: 16,
                                  color: Color.fromRGBO(21, 21, 21, 1.0),
                          ), 
                        ),

                        Text(
                          _category,
                          style: const TextStyle(
                                  fontWeight: FontWeight.w700,
                                  fontSize: 16,
                                  color: Color.fromRGBO(51, 155, 124, 1.0),
                          ), 
                        ),
                      
                      ]
                    ),
                  ),

                  Expanded(
                    flex: 15,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        const Text(
                          'SubCategory  ',
                          style: TextStyle(
                              fontWeight: FontWeight.w500,
                              fontSize: 16,
                              color: Color.fromRGBO(21, 21, 21, 1.0),
                            ), 
                        ),

                        Text(
                          _subCategory,
                          style: const TextStyle(
                                  fontWeight: FontWeight.w700,
                                  fontSize: 16,
                                  color: Color.fromRGBO(51, 155, 124, 1.0),
                              ), 
                        ),
                      ]
                    ),
                  ),
                ]
              ),
              const Divider(),
              const SizedBox(
                height: 2,
              ),                          

              SingleChildScrollView(
                child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: _productDetail.productVariables.map((productVariable) => InventoryProductDetailVariablePage(
                      userProductId: _productDetail.userProductId, productVariable: productVariable, updateShowImages: updateShowImages)
                    ).toList(),
                ),
              ),
            ]
        ),
      ),
    );
    }
}
