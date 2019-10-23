Things that still need working on:

- Logging (preferrably through a non static member)
- Fixing a bug with the location inventory being added to rather than depleted (fix attempt 10/23/19)
- Make css more customized rather than default bootstrap
- Fix AddProduct logic so that an order can only increase the quantity of a particular item in a cart up to 5
    (currently only checks for adding up to 5 quantity of a product at a time, but does not check with the cart to see if 5 max has already been added)