export class PaginableList {
  constructor(items = [], itemsPerPage = 10) {
    this._items = Array.isArray(items) ? items : [];
    this._itemsPerPage = Math.max(1, parseInt(itemsPerPage) || 10);
    this._currentPage = 1;
  }

  // Get current page number
  get CurrentPage() {
    return this._currentPage;
  }

  // Get number of items per page
  get ItemsPerPage() {
    return this._itemsPerPage;
  }

  // Get total number of pages
  get TotalPageQuantity() {
    return Math.ceil(this._items.length / this._itemsPerPage);
  }

  // Get total items
  get getLength() {
    return this._items.length;
  }
  // Get items currently visible on the page
  get CurrentShowedItems() {
    const startIndex = (this._currentPage - 1) * this._itemsPerPage;
    const endIndex = Math.min(startIndex + this._itemsPerPage, this._items.length);
    return this._items.slice(startIndex, endIndex);
  }

  // Change to a specific page
  ChangePage(page) {
    const pageNumber = parseInt(page);
    if (isNaN(pageNumber) || pageNumber < 1 || pageNumber > this.TotalPageQuantity) {
      console.error(`Invalid page number: ${page}. Must be between 1 and ${this.TotalPageQuantity}`);
      return false;
    }
    this._currentPage = pageNumber;
    return true;
  }

  // Additional helpful methods
  NextPage() {
    if (this._currentPage < this.TotalPageQuantity) {
      this._currentPage++;
      return true;
    }
    return false;
  }

  PreviousPage() {
    if (this._currentPage > 1) {
      this._currentPage--;
      return true;
    }
    return false;
  }

  // Update the items in the list
  UpdateItems(newItems) {
    this._items = Array.isArray(newItems) ? newItems : [];
    // Reset to first page if current page would be invalid
    if (this._currentPage > this.TotalPageQuantity && this.TotalPageQuantity > 0) {
      this._currentPage = 1;
    }
  }

  // Change items per page
  SetItemsPerPage(newItemsPerPage) {
    const newValue = Math.max(1, parseInt(newItemsPerPage) || 10);
    if (newValue !== this._itemsPerPage) {
      // Calculate what item index we're currently viewing to try to stay near it
      const firstItemIndex = (this._currentPage - 1) * this._itemsPerPage;
      this._itemsPerPage = newValue;
      this._currentPage = Math.floor(firstItemIndex / this._itemsPerPage) + 1;
    }
  }
}

// Example usage:
/*
const data = Array.from({length: 100}, (_, i) => `Item ${i + 1}`);
const pager = new PaginableList(data, 5);

console.log(pager.CurrentPage); // 1
console.log(pager.ItemsPerPage); // 5
console.log(pager.TotalPageQuantity); // 20
console.log(pager.CurrentShowedItems); // ["Item 1", "Item 2", "Item 3", "Item 4", "Item 5"]

pager.ChangePage(3);
console.log(pager.CurrentShowedItems); // ["Item 11", "Item 12", "Item 13", "Item 14", "Item 15"]

pager.NextPage();
console.log(pager.CurrentPage); // 4

pager.SetItemsPerPage(10);
console.log(pager.ItemsPerPage); // 10
console.log(pager.CurrentShowedItems); // Items near where we were before changing page size
*/