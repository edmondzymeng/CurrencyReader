

# CurrencyReader

This is a simple tool used to convert currency number into English presentation, based on ASP.NET MVC 5.

## Installation

- Download and extract released zip file.
- Open IIS Manager, create an application pool with **.NET CLR version v4.0** or higher and **Integrated Pipeline Mode**.
- Host the folder in the application pool, assign a port (e.g. *8080*) for it and leave other settings as default.
- Open your browser, visit `http://localhost:[port]` to have a try!

## Usage

In the **Currency** input, enter a number in the range `[-99999999999999999999999999.99, 99999999999999999999999999.99]` to get the English presentation.

Also make sure no more than **28** digits to input, or the application would refuse to convert this number.

Different formats of input are allowed:

- **General format**: e.g. *130423.05* or *3.1415926*
- **With Thousandths**: e.g. *130,423.05* or *1,000,010,010*
- **Scientific Notation**: e.g. *1.151E+15*

## Convertion Rule

If it is a negative number, there would be a `Minus` before the digit part;
If it is zero, just return `Zero`;
If it is a positive number:
* If it is less than 1 dollar, return `xx Cent(s)`
* If it is larger than or equal to 1 dollar(without decimal), return `xxx Dollar(s)`
* If it is larger than or equal to 1 dollar(with decimal), return `xxx Dollar(s) and xx Cent(s)`
* Attention:
	* Mind **plural form**
	* **Round** the number to accounting number format
	* No *and* after *Thousand*, *Million*, *Billion*, etc. while there should be *and* after *Hundred* if the following section before the next thousandth is not *00*, e.g.:
		* *130,423* is *One Hundred and Thirty Thousand Four Hundred and Twenty Three Dollars*
		* *1,000,010,000* is *One Billion Ten Thousand Dollars*;
		* **Exception**: If the hundreds place is *0*, but the tens place or ones place is not,
                  there should be *and* after the last *Thousand*, *Million* or *Billion*, etc., e.g.:
			* *130,023* is *One Hundred and Thirty Thousand and Twenty Three Dollars*

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)