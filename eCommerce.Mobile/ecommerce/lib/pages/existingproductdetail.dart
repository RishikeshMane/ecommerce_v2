import 'package:ecommerce/services/existingproduct.service.dart';
import 'package:flutter/material.dart';

import 'package:ecommerce/pages/subpages/existingproductdetailvariable.dart';
import 'package:ecommerce/services/product.service.dart';
import 'package:ecommerce/models/productdetail.dart';
import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/utils/util.dart';
import 'package:localstorage/localstorage.dart';

class ExistingProductDetailPage extends StatefulWidget {
  const ExistingProductDetailPage({super.key, required this.title});

  final String title;

  @override
  State<ExistingProductDetailPage> createState() => ExistingProductDetailPageState();
}

class ExistingProductDetailPageState extends State<ExistingProductDetailPage> {

  ProductDetail _productDetail = ProductDetail();
  String _category = '';
  String _subCategory = '';

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

  Future<void> onAddProduct() async{
    String mobileNo = localStorage.getItem('mobileno').toString();
    String userProductId = Util.getUserProductId();

    ExistingProductService eps = ExistingProductService();

    await eps.copyProduct(mobileNo, _productDetail.userProductId, userProductId);
    if (eps.productAdded == true)
    {
      _productCopiedDialog();
    }
  }

  Future<void> _productCopiedDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Product Added !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('Product Added Successfully.',
                    style: TextStyle(
                    fontSize: 20,
                ),
                ),
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              style: Style.enableButton,
              onPressed: () {
                Navigator.pop(context, false);
                ///Navigator.of(context).pop();
                ///Navigator.pop(context);                
              },
              child: const Text(' Ok '),
            ),
          ],
        );
      },
    );
  }


  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {

    _productDetail = ModalRoute.of(context)!.settings.arguments as ProductDetail;
    updateCategory();
    updateSubCategory();
    updateProductVariable();

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
                child: Text(
                  _productDetail.title,
                  style: const TextStyle(
                    fontWeight: FontWeight.w700,
                    fontSize: 16,
                    color: Color.fromRGBO(51, 155, 124, 1.0),
                  ),
                  maxLines: 6,               
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
                child: Text(
                  _productDetail.description,
                  style: const TextStyle(
                    fontWeight: FontWeight.w700,
                    fontSize: 16,
                    color: Color.fromRGBO(51, 155, 124, 1.0),
                  ),
                  maxLines: 21,                 
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

              Center(
                child:
                  ElevatedButton(
                    style: Style.borderFlatButton,
                    onPressed: (){
                      setState((){
                        onAddProduct();
                      });
                    },
                    child: const Text(
                              '( + ) Add Product to My Inventory',
                              style: TextStyle(
                                fontWeight: FontWeight.w800,
                                fontSize: 16,
                              ),
                    ),
                  )
              ),

              const SizedBox(
                height: 2,
              ),                            

              SingleChildScrollView(
                child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: _productDetail.productVariables.map((productVariable) => ExistingProductDetailVariablePage(
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
