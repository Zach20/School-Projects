function getReceipt() {
	var text1 = "<h3>You Ordered:</h3>";
	var runningTotal = 0;
	var sizeTotal = 0;
	var sizeArray = document.getElementsByClassName("size");

	for (var i = 0; i < sizeArray.length; i++) {
		if (sizeArray[i].checked) {
			var selectedSize = sizeArray[i].value;
			text1 = text1+selectedSize+"<br>";
		}
	}
	if (selectedSize === "Personal Pizza") {
		sizeTotal = 6;
	} else if (selectedSize === "Medium Pizza") {
		sizeTotal = 10;
	} else if (selectedSize === "Large Pizza") {
		sizeTotal = 14;
	} else if (selectedSize === "Extra Large Pizza") {
		sizeTotal = 16;
	}
	runningTotal = sizeTotal;
	console.log(selectedSize+" = $"+sizeTotal+".00");
	console.log("size text1: "+text1);
	console.log("subtotal: $"+runningTotal+".00");
	getMeat(runningTotal,text1);
};

function getMeat(runningTotal,text1) {
	var meatTotal = 0;
	var selectedMeat = [];
	var meatArray = document.getElementsByClassName("meats");
	for (var j = 0; j < meatArray.length; j++) {
		if (meatArray[j].checked) {
			selectedMeat.push(meatArray[j].value);
			console.log("selected meat item: ("+meatArray[j].value+")");
			text1 = text1+meatArray[j].value+"<br>";
		}
	}
	var meatCount = selectedMeat.length;
	if (meatCount > 1) {
		meatTotal = (meatCount - 1);
	} else {
		meatTotal = 0;
	}
	runningTotal = (runningTotal + meatTotal);
	console.log("total selected meat items: "+meatCount);
	console.log(meatCount+" meat - 1 free meat = "+"$"+meatTotal+".00");
	console.log("meat text1: "+text1);
	console.log("Purchase Total: "+"$"+runningTotal+".00");
	getVeggies(runningTotal,text1);
};

function getVeggies(runningTotal,text1) {
	var veggieTotal = 0;
	var selectedveggies = [];
	var veggieArray = document.getElementsByClassName("veggies");
	for (var h = 0; h < veggieArray.length; h++) {
		if (veggieArray[h].checked) {
			selectedveggies.push(veggieArray[h].value);
			console.log("selected veggie item: ("+veggieArray[h].value+")");
			text1 = text1+veggieArray[h].value+"<br>";
		}
	}
	var veggieCount = selectedveggies.length;
	if (veggieCount > 1) {
		veggieTotal = (veggieCount - 1);
	} else {
		veggieTotal = 0;
	}
	runningTotal = (runningTotal + veggieTotal);
	console.log("total selected veggie items: "+veggieCount);
	console.log(veggieCount+" veggie - 1 free veggie = "+"$"+veggieTotal+".00");
	console.log("veggie text1: "+text1);
	console.log("Purchase Total: "+"$"+runningTotal+".00");
	getCrusts(runningTotal,text1);
};

function getCrusts(runningTotal,text1) {
	var crustTotal = 0;
	var crustArray = document.getElementsByClassName("crusts");

	for (var r = 0; r < crustArray.length; r++) {
		if (crustArray[r].checked) {
			var selectedcrust = crustArray[r].value;
			text1 = text1+selectedcrust+"<br>";
		}
	}
	if (selectedcrust === "Cheese Stuffed Crust") {
		crustTotal = 3;
	}
	runningTotal = (runningTotal + crustTotal);
	console.log(selectedcrust+" = $"+crustTotal+".00");
	console.log("crust text1: "+text1);
	console.log("subtotal: $"+runningTotal+".00");
	getSauces(runningTotal,text1);
};

function getSauces(runningTotal,text1) {
	var sauseArray = document.getElementsByClassName("Sauces");

	for (var s = 0; s < sauseArray.length; s++) {
		if (sauseArray[s].checked) {
			var selectedsause = sauseArray[s].value;
			text1 = text1+selectedsause+"<br>";
		}
	}
	runningTotal = (runningTotal);
	getCheese(runningTotal,text1);
};

function getCheese(runningTotal,text1) {
	var cheeseTotal = 0;
	var cheeseArray = document.getElementsByClassName("Cheese");

	for (var c = 0; c < cheeseArray.length; c++) {
		if (cheeseArray[c].checked) {
			var selectedcheese = cheeseArray[c].value;
			text1 = text1+selectedcheese+"<br>";
		}
	}
	if (selectedcheese === " Extra Cheese") {
		cheeseTotal = 3;
	}
	runningTotal = (runningTotal + cheeseTotal);
	console.log(selectedcheese+" = $"+cheeseTotal+".00");
	console.log("cheese text1: "+text1);
	console.log("subtotal: $"+runningTotal+".00");
	document.getElementById("showText").innerHTML=text1;
	document.getElementById("totalPrice").innerHTML = "</h3>Total: <strong>$"+runningTotal+".00"+"</strong></h3>";
};