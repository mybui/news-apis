import React, { useState, useEffect } from "react";
import Spinner from "./Spinner.js";

export function time(datetime) {
  const currentDate = new Date();
  const targetDate = new Date(datetime);

  const options = { year: "numeric", month: "short", day: "numeric" };
  return targetDate.toLocaleDateString(undefined, options);
}

function formatNumber(num) {
    if (num >= 1000000000) {
        return (num / 1000000000).toFixed(1) + 'B';
    }
    if (num >= 1000000) {
        return (num / 1000000).toFixed(1) + 'M';
    }
    if (num >= 1000) {
        return (num / 1000).toFixed(1) + 'K';
    }
    return num.toString();
}

var currency = "";

const Item = ({left, right, background, ...props}) => {
	return <div className={`flow-root ${background}`}>
	  <h2 className="float-left"> {left} </h2>
	  <h2 className="float-right"> {formatNumber(right)} {currency} </h2>
       </div>
}

const ItemNoFormat = ({left, right, background, ...props}) => {
	return <div className={`flow-root ${background}`}>
	  <h2 className="float-left"> {left} </h2>
	  <h2 className="float-right"> {right} {currency} </h2>
       </div>
}

export function formatFinancials(item) {
  return (
    <div className="w-full">
      <h2 className="heading-h2-bold text-stuct-600 text-14px pb-2">
	Stock
      </h2>
	  <div className="w-full flow-root text-struct-700">
	      <ItemNoFormat left="Open" right={item.stock.open} background="bg-white"/>
	      <ItemNoFormat left="High" right={item.stock.high} background="bg-neutral-100"/>
	      <ItemNoFormat left="Low" right={item.stock.low} background="bg-white"/>
	      <ItemNoFormat left="Close" right={item.stock.close} background="bg-neutral-100"/>
	  </div>
      <h2 className="heading-h2-bold text-stuct-600 text-14px py-2">
	  Financials
      </h2>
	  <div className="text-struct-700">
	      <div className={`w-full flow-root bg-white`}>
		  <h2 className="float-left"> Date </h2>
		  <h2 className="float-right"> {time(item.financials.date)} </h2>
	      </div>
	      <Item left="Revenue" right={item.financials.revenue} background="bg-neutral-100"/>
	      <Item left="Total Assets" right={item.financials.totalAssets} background="bg-white"/>
	      <Item left="Cash" right={item.financials.cash} background="bg-neutral-100"/>
	      <Item left="Cash & Equivalent" right={item.financials.cashAndEquivalent} background="bg-white"/>
	      <Item left="Inventory" right={item.financials.inventory} background="bg-neutral-100"/>
	      <Item left="Accounts Payable" right={item.financials.accountsPayable} background="bg-white"/>
	  </div>
    </div>
  );
}

/**
 * Sievo-styled financials component.
 *
 * @param {{
 *  label: string;
 *  inputs: string[];
 *  className?: string;
 *  inputClassName?: string;
 *  placeholder?: string;
 * } & import("react").ComponentPropsWithoutRef<typeof ComboBox>} props
 *
 * @see https://react-spectrum.adobe.com/react-aria/ComboBox.html
 */

export const Financials = ({ supplier, ...props }) => {
  const [financials, setFinancials] = useState([]);
  const [apiResponseError, setAPIResponseError] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (supplier) {
      setIsLoading(true);
      fetch(
        "/api/finance?supplierName=" +
          encodeURIComponent(supplier)
      )
        .then(async (response) => {
          setAPIResponseError(null);
          setFinancials([]);
          const json = await response.json();
          if (!response.ok) {
            throw new Error(json.error);
          } else {
            const financials = formatFinancials(json);
            setFinancials(financials);
	    currency=json.financials.reportedCurrency;
          }
        })
        .catch((error) => {
          const financials = formatFinancials({});
          setFinancials(financials);
          setAPIResponseError(error.message);
        })
        .finally(() => {
          setIsLoading(false);
        });
    }
  }, [supplier]);

  return isLoading ? (
    <div className="flex justify-center py-20 h-full">
      <Spinner />
    </div>
  ) : (
    <div className="space-y-5">
      {financials}
  <h2>{apiResponseError}</h2>
    </div>
  );
};

export default Financials;
