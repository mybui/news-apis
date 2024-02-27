import React, { useState, useEffect } from "react";
import { Dropdown } from "./Dropdown.js";
import { List } from "./List.js";
import { Financials } from "./Financials.js";

/**
 * Sievo-styled component with three dropdowns: suppliers, countries, languages
 */
export const News = () => {
  const [supplierNames, setSupplierNames] = useState([]);
  const [countryNames, setCountryNames] = useState([]);
  const [languages, setLanguages] = useState([]);
  const [supplierError, setSupplierError] = useState("");
  const [countryError, setCountryError] = useState("");
  const [languageError, setLanguageError] = useState("");
  const [selectedSupplier, setSelectedSupplier] = useState();
  const [selectedCountry, setSelectedCountry] = useState();
  const [selectedLanguage, setSelectedLanguage] = useState();
  const [supplierDropDownText, setSupplierDropDownText] = useState("");
  const [countryDropDownText, setCountryDropDownText] = useState("");

  useEffect(() => {
    fetch("/api/suppliergroup")
      .then((response) => {
        if (!response.ok) {
          setSupplierError("Couldn't fetch supplier data");
        }
        return response.json();
      })
      .then((data) => {
        const names = data.map((supplier) => supplier.name);
        setSupplierNames(names);
      })
      .catch((error) => {
        setSupplierError("Error with supplier data");
      });
  }, []);

  useEffect(() => {
    fetch("/api/countrylist")
      .then((response) => {
        if (!response.ok) {
          setCountryError("Couldn't fetch country data");
        }
        return response.json();
      })
      .then((data) => {
        const names = data.map((country) => country.alpha2);
        setCountryNames(names);
      })
      .catch((error) => {
        setCountryError("Error with country data");
      });
  }, []);

  useEffect(() => {
    fetch("/api/languagelist")
      .then((response) => {
        if (!response.ok) {
          setLanguageError("Couldn't fetch language data");
        }
        return response.json();
      })
      .then((data) => {
        setLanguages(data);
      })
      .catch((error) => {
        setLanguageError("Error with language data");
      });
  }, []);

  return (
    <div className="flex flex-col h-screen py-4 px-8">
      <div className="flex gap-3 flex-col border border-struct-200 h-full mx-auto w-full + max-w-lg p-4 rounded">
        <h2 className="heading-h2-bold text-struct-700 text-[22px]">
          News and alerts
        </h2>
        {supplierError && <p>{supplierError}</p>}
        {countryError && <p>{countryError}</p>}
        {languageError && <p>{languageError}</p>}
        {!supplierError && !countryError && !languageError && (
          <div className="flex gap-2 py-2">
            <Dropdown
              label={"Choose supplier"}
              inputs={supplierNames}
              placeholder={"Select"}
              className="w-[65%]"
              inputValue={supplierDropDownText}
              onInputChange={(name) => {
                setSupplierDropDownText(name);
                if (
                  supplierNames.some(
                    (supplierName) =>
                      supplierName.toLocaleLowerCase() ===
                      name.toLocaleLowerCase()
                  )
                ) {
                  setSelectedSupplier(name);
                }
              }}
            />
            <Dropdown
              label={"Country"}
              inputs={countryNames}
              placeholder={""}
              className="w-[30%]"
              inputValue={countryDropDownText}
              onInputChange={(name) => {
                setCountryDropDownText(name);
                if (
                  name === "" ||
                  countryNames.some(
                    (countryName) =>
                      countryName.toLocaleLowerCase() ===
                      name.toLocaleLowerCase()
                  )
                ) {
                  setSelectedCountry(name);
                }
              }}
            />
            <Dropdown
              label={"Language"}
              inputs={languages.map((language) => language.name)}
              placeholder={""}
              className="w-[45%]"
              inputValue={selectedLanguage ? selectedLanguage.name : ""}
              onInputChange={(name) => {
                const language = languages.find((lang) => lang.name === name);
                setSelectedLanguage(
                  language ? language : { name: name, code: "" }
                );
              }}
            />
          </div>
        )}
        <div className="overflow-y-scroll h-full">
	<Financials supplier={selectedSupplier}></Financials>
           <List
             className=""
             supplier={selectedSupplier}
             country={selectedCountry}
             language={selectedLanguage ? selectedLanguage.code : ""}
           />
        </div>
      </div>
    </div>
  );
};
