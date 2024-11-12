import 'package:ecommerce/models/categorydetail.dart';
import 'package:ecommerce/models/productdetail.dart';
import 'package:ecommerce/services/existingproduct.service.dart';
import 'package:ecommerce/services/product.service.dart';
import 'package:flutter/material.dart';

import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/utils/util.dart';

class Products extends DataTableSource{

  List<ProductDetail> productDetails = [];
  CategoryList categories = CategoryList();
  late void Function(int index, bool check) setCheck;
  late void Function(int index) onProductDetail;

  Products({required this.productDetails, required this.categories, required this.setCheck, required this.onProductDetail});

  @override
  int get rowCount => productDetails.length;

  @override
  DataRow? getRow(int index) {

    Color cl = const Color.fromRGBO(51, 155, 124, 1.0);
    TextStyle ts = TextStyle(fontWeight: FontWeight.w800, color: cl);
    TextStyle uts = TextStyle(fontWeight: FontWeight.w800, color: cl, decoration: TextDecoration.underline);
    
    if (index % 2 == 0)
    {
      cl = const Color.fromRGBO(46, 46, 46, 1);
      ts = TextStyle(fontWeight: FontWeight.w600, color: cl);
      uts = TextStyle(fontWeight: FontWeight.w600, color: cl, decoration: TextDecoration.underline);
    }

    String category = Util.getCategoryName(productDetails[index].categoryLinkId, categories);
    String subCategory = Util.getSubCategoryName(productDetails[index].categoryLinkId, productDetails[index].subCategoryLinkId, categories);
    
    return DataRow(
          cells: <DataCell>[
            DataCell(
                    Text(('  ${(index+1).toString()}'),
                      style: ts,
                      )
                    ),
            
            DataCell(IconButton(
              color: cl,
              onPressed: (){
                onProductDetail(index);
            }, icon: const Icon(
              Icons.table_view,
            ),)),
            DataCell(Checkbox(
                value: productDetails[index].isSelect,
                fillColor: WidgetStateProperty.resolveWith((states) {
                            if (!states.contains(WidgetState.selected)) {
                              return Style.textBackground;
                            }
                            return null;
                          }),
                onChanged: (show) => setCheck(index, show!),
                /**
                {
                  ///productDetails[index].isSelect = show!;
                  setCheck(index, show!);
                },
                */
            ),),            
            DataCell(
              ElevatedButton(
                onPressed: (){
                  onProductDetail(index);
                },
                style: const ButtonStyle(
                  backgroundColor: WidgetStatePropertyAll(Color.fromRGBO(228, 228, 228, 1.0)),
                ),
                child: Text(productDetails[index].userProductId,
                          style: uts,            
                        ),
              ),
            ),
            DataCell(Text(category,
                          style: ts,
            )),
            DataCell(Text(subCategory,
                          style: ts,
            )),            
            DataCell(Text(productDetails[index].title.substring(0, productDetails[index].title.length > 15 ? 15 : productDetails[index].title.length),                       
                        style: ts,
            )),
            DataCell(Text(productDetails[index].description.substring(0, productDetails[index].description.length > 15 ? 15 : productDetails[index].description.length),
                        style: ts,
            )),                     
          ],
        );
  }

  sort(columnIndex, ascending){
    if (ascending)
    {
      productDetails.sort((a, b) => b.categoryLinkId.compareTo(a.categoryLinkId));
    }
    else{
      productDetails.sort((a, b) => a.categoryLinkId.compareTo(b.categoryLinkId));
    } 
  }

  @override
  bool get isRowCountApproximate => false;

  @override
  int get selectedRowCount => 0;
}

class ExistingProductPage extends StatefulWidget {
  const ExistingProductPage({super.key, required this.title});

  final String title;

  @override
  State<ExistingProductPage> createState() => ExistingProductPageState();
}

class ExistingProductPageState extends State<ExistingProductPage> {

  List<String> _productIds = ['All'];
  CategoryList _categories = CategoryList();
  List<ProductDetail> _productDetails = [];
  List<String> _toBeAddedProductIds = [];

  List<ProductDetail>? _dataProductDetails=[];
  int _sortColumnIndex=0;
  bool _sortAscending = true;
  String _selectedProductId = 'All';
  dynamic _args;

  @override
  void initState() {
    updateProducts();
    super.initState();
  }  

  Future<void> updateProducts() async {
    if (_productDetails.isNotEmpty) return;
    
    String productId = Util.getUserProductId();
    ExistingProductService eps = ExistingProductService();
    ProductService ps = ProductService();
    await eps.getExistingProductIds(productId);
    await ps.getCategories();
    await eps.getExistingProductDetails(productId);

    try{
      setState((){
        if (_productIds.length == 1 && eps.productIds.isNotEmpty)
        {
          _productIds.addAll(eps.productIds);
        }
        _categories = ps.categories;
        _productDetails = eps.productDetails;
        _dataProductDetails = _productDetails;
      });
    }
    catch(e){}
  }

  void updateSelectedIdProducts(){
    _dataProductDetails = _productDetails;

    if (_selectedProductId != 'All')
    {
      _dataProductDetails = _productDetails.where((productDetail) => productDetail.userProductId.contains(_selectedProductId)).toList();
    }
  }

  Future<void> onAddProducts() async{
    String mobileNo = _args['mobileno'].toString();
    String userProductId = Util.getUserProductId();

    ExistingProductService eps = ExistingProductService();
    await eps.copyProducts(mobileNo, _toBeAddedProductIds, userProductId);
    if (_toBeAddedProductIds.isNotEmpty)
    {
      _productCopiedDialog();
    }else{
      _productSelectDialog();
    }
  }

  void _setCheck(int index, bool check){
    setState(() {
      _dataProductDetails?[index].isSelect = check;

      if (check){
        _toBeAddedProductIds.add(_dataProductDetails![index].userProductId);
      }else{
        _toBeAddedProductIds.removeAt(_toBeAddedProductIds.indexOf(_dataProductDetails![index].userProductId));
      }
    });
  }

  void _onProductDetail(int index){
    ProductDetail productDetail = _dataProductDetails![index];
    productDetail.category = _categories;
    Navigator.pushNamed(context, '/existingproductdetail', arguments: productDetail);
  }

  Future<void> _productCopiedDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Product(s) Added !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('Product(s) Added Successfully.',
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

  Future<void> _productSelectDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Select Product(s) !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('Please select Product(s).',
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

  List<DataColumn> createColumns(){
    return [
              const DataColumn(
                label: Text(
                        '  Id',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                        ),
              ),
              const DataColumn(
                label: Text(
                        'View',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                        ),
              ),
              const DataColumn(
                label: Text(
                        '(+) Add',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                        ),
              ),                           
              DataColumn(
                label: const Text(
                        'Product Id',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                        ),

                onSort: (columnIndex, ascending){
                  setState(() {
                    if (ascending){
                      _dataProductDetails!.sort((a, b) => a.userProductId.compareTo(b.userProductId));
                    }
                    else {
                      _dataProductDetails!.sort((a, b) => b.userProductId.compareTo(a.userProductId));
                    }
                  }
                  );
                  _sortColumnIndex = columnIndex;
                  _sortAscending = ascending;                  
                },
              ),
              DataColumn(
                label: const Text('Category',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),                 
                      ),
                onSort: (columnIndex, ascending){
                  setState(() {
                    if (ascending)
                    {
                      _dataProductDetails!.sort((a, b) ///=> a.categoryLinkId.compareTo(b.categoryLinkId)
                      {
                          String categorydetailA = Util.getCategoryName(a.categoryLinkId, _categories);
                          String categorydetailB = Util.getCategoryName(b.categoryLinkId, _categories);
                          return categorydetailA.compareTo(categorydetailB);                      
                      }
                      );
                    }
                    else{
                      _dataProductDetails!.sort((a, b) ///=> a.categoryLinkId.compareTo(b.categoryLinkId)
                      {
                          String categorydetailA = Util.getCategoryName(a.categoryLinkId, _categories);
                          String categorydetailB = Util.getCategoryName(b.categoryLinkId, _categories);
                          return categorydetailB.compareTo(categorydetailA);
                      }
                      );
                    }                
                    _sortColumnIndex = columnIndex;
                    _sortAscending = ascending;                      
                  });  
                },                    
              ),
              const DataColumn(
                label: Text('SubCategory',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),                 
                      ),
              ),                                                                                                           
              const DataColumn(
                label: Text(
                        'Title',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),                        
                        ),                
              ),
              const DataColumn(
                label: Text(
                        'Description',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),                        
                        ),
              ),
            ];
  }

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {

    _args = ModalRoute.of(context)!.settings.arguments;

    updateProducts();
    ShowImage().resetShowImage();

    return Scaffold(
      resizeToAvoidBottomInset: true,
      backgroundColor: Style.pageBackground,
      appBar: AppBar(
        title: Text(
          widget.title,
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
              scrollDirection: Axis.vertical,
              child: Column (
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

                                          Row(
                                            mainAxisAlignment: MainAxisAlignment.start,
                                            children: <Widget>[
                                              const Text(
                                                      'ProductId:   ',
                                                      style: TextStyle(
                                                        fontWeight: FontWeight.w800,
                                                        fontSize: 16,
                                                      ),
                                              ),
                                            
                                              Container(
                                                padding: const EdgeInsets.fromLTRB(5, 0, 5, 0),
                                                height: 35.0,
                                                decoration: BoxDecoration(
                                                              color: Colors.white, borderRadius: BorderRadius.circular(2),
                                                              border: Border.all(
                                                                color: const Color.fromRGBO(201, 201, 201, 1.0),
                                                              ),
                                                            ),
                                                child: DropdownButton<String>(
                                                    value: _selectedProductId,
                                                    dropdownColor: const Color.fromRGBO(201, 201, 201, 1.0),
                                                    elevation: 10,
                                                    items: _productIds.map<DropdownMenuItem<String>>((String value) {
                                                            return DropdownMenuItem<String>(
                                                              value: value,
                                                              child: Text(value),
                                                            );
                                                          }).toList(),
                                                    onChanged: (String? selected){
                                                    setState(() {
                                                      _selectedProductId = selected!;
                                                      updateSelectedIdProducts();
                                                      });
                                                    },
                                                ),
                                            ),
                                            ]
                                          ),

                                          const SizedBox(
                                            height: 15,
                                          ),                                          
                                            
                                          Row(
                                            mainAxisAlignment: MainAxisAlignment.center,
                                            children: <Widget>[
                                            Expanded(
                                            flex: 10,                                       
                                            child: ElevatedButton(
                                                            style: Style.enableFlatButton,
                                                            onPressed: (){
                                                              onAddProducts();
                                                            },
                                                            child: const Text(
                                                                            '( + ) Add Products to My Inventory',
                                                                            style: TextStyle(
                                                                              fontWeight: FontWeight.w800,
                                                                              fontSize: 16,
                                                                            ),
                                                            ),
                                            ),
                                            ),

                                            ]
                                          ),                                                                                    

                                          const SizedBox(
                                            height: 21,
                                          ),                                          

                                          Theme(
                                            data: Theme.of(context).copyWith(
                                              dataTableTheme: DataTableThemeData(
                                                headingRowColor: WidgetStateProperty.resolveWith<Color>(
                                                    (Set<WidgetState> states) {
                                                    return const Color.fromRGBO(51, 155, 124, 1.0);
                                                }),
                                                dataRowColor: WidgetStateProperty.resolveWith<Color>(
                                                    (Set<WidgetState> states) {
                                                    return Style.pageBackground;
                                                }),
                                                headingTextStyle: const TextStyle(
                                                  color: Color.fromRGBO(21, 21, 21, 1.0),
                                                  fontWeight: FontWeight.bold,
                                                  fontSize: 17,
                                                ),
                                                dataTextStyle: const TextStyle(
                                                  color: Color.fromRGBO(21, 21, 21, 1.0),
                                                  fontSize: 15,
                                                ),
                                                dividerThickness: 2,                                      
                                              ),
                                              textTheme: const TextTheme(
                                                                  bodySmall: TextStyle(color: Color.fromRGBO(21, 21, 21, 1)),
                                                                  bodyMedium: TextStyle(color: Color.fromRGBO(21, 21, 21, 1)),
                                                                  bodyLarge: TextStyle(color: Color.fromRGBO(21, 21, 21, 1)),
                                                                  ), 
                                            ),
                                            child: PaginatedDataTable(
                                              columns: createColumns(),
                                              source: Products(productDetails: _dataProductDetails!, categories: _categories, setCheck: _setCheck, onProductDetail: _onProductDetail),
                                              /**
                                              header: const Center(
                                                child: Text(
                                                  'Other User products',
                                                  style: TextStyle(
                                                    color: Color.fromRGBO(51, 155, 124, 1.0),
                                                    letterSpacing: 1.0,
                                                    fontWeight: FontWeight.bold,
                                                    fontSize: 25,
                                                  ),
                                                ),
                                              ),
                                              */

                                              columnSpacing: 22,
                                              horizontalMargin: 5,
                                              showFirstLastButtons: true,
                                              rowsPerPage: 15,
                                              sortColumnIndex: _sortColumnIndex,
                                              sortAscending: _sortAscending,
                                              showEmptyRows: true,
                                              arrowHeadColor: const Color.fromRGBO(21, 21, 21, 1),
                                            ),
                                          ),
                                        ],
                    ),
                      
              ),
            
    );
  }
}
