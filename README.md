

# CurrencyReader

This is a simple tool for converting currency number to English representation, based on ASP.NET MVC 5.

## Installation

- Download and extract released zip file.
- Open IIS Manager and create an application pool with **.NET CLR version v4.0** or higher and **Integrated Pipeline Mode**.
- Host the folder in the application pool, assign it a port (e.g. *8080*) and keep other default settings.
- Open the browser, visit `http://localhost:[port]` and try it out!

## Usage

In the **Currency** input, enter a number in the range `[-99999999999999999999999999.99, 99999999999999999999999999.99]` to get the English representation.

Also, be sure to enter no more than **28** digits or the application will refuse to convert this number.

Allow different input formats to be used:

- **General format**: e.g. *130423.05* or *3.1415926*
- **With Comma as Thousands Separator**: e.g. *130,423.05* or *1,000,010,010*
- **Scientific Notation**: e.g. *1.151E+15* or *7.8E10*

## Convertion Rule

If it is a negative number, there would be a `Negative` before the digit part representaion;
If it is zero, it will return `Zero`;
If it is a positive number, consider the following circumstances:
* When it is less than 1 dollar, return `xx Cent(s)`
* When it is larger than or equal to 1 dollar(without decimal), return `xxx Dollar(s)`
* When it is larger than or equal to 1 dollar(with decimal), return `xxx Dollar(s) and xx Cent(s)`
* Note:
	* Pay attention to **plural form** for `Dollar/Dollars` or `Cent/Cents`
	* **Round** the number to two decimal places
	* Do **NOT** add `and` after thousands representation (`Thousand`, `Million`, `Billion`, etc.), add `and` between `Hundred` and the next tens or ones place, e.g.:
		* *1,000,010,000* is *One Billion Ten Thousand Dollars*
		* *130,423* is *One Hundred and Thirty Thousand Four Hundred and Twenty Three Dollars*
		* However, there is an **Exception**: If the hundreds place is `0`, but the tens place or ones place is not `0`, there should be *and* after the last thousands representation, e.g.:
			* *130,023* is *One Hundred and Thirty Thousand and Twenty Three Dollars*
* Thousands representation:

	Name | Number of Zeros | Groups of (3) Zeros
	:-: | :-: | :-:
	Thousand | 3 | 1|
	Million | 6 | 2|
	Billion | 9 | 3|
	Trillion | 12 | 4|
	Quadrillion | 15 | 5|
	Quintillion | 18 | 6|
	Sextillion | 21 | 7|
	Septillion | 24 | 8|
	Octillion | 27 | 9|
	... | ... | ...

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
