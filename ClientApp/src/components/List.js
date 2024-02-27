import React, { useState, useEffect } from "react";
import Spinner from "./Spinner.js";

import { Button } from "react-aria-components";

// Helper function to format datetime

export function timeAgo(datetime) {
  const currentDate = new Date();
  const targetDate = new Date(datetime);

  const timeDifference = currentDate - targetDate;
  const seconds = Math.floor(timeDifference / 1000);
  const minutes = Math.floor(seconds / 60);
  const hours = Math.floor(minutes / 60);
  const days = Math.floor(hours / 24);

  if (days > 7) {
    // If more than 7 days, display the date
    const options = { year: "numeric", month: "short", day: "numeric" };
    return targetDate.toLocaleDateString(undefined, options);
  } else if (days > 0) {
    // Display days ago
    return days === 1 ? "1 day ago" : `${days} days ago`;
  } else if (hours > 0) {
    // Display hours ago
    return hours === 1 ? "1 hour ago" : `${hours} hours ago`;
  } else if (minutes > 0) {
    // Display minutes ago
    return minutes === 1 ? "1 minute ago" : `${minutes} minutes ago`;
  } else {
    // Display seconds ago
    return seconds === 1 ? "1 second ago" : `${seconds} seconds ago`;
  }
}

export function formatNewsItem(item) {
  return (
    <ul className="leading-6 w-full max-w-md py-2">
      <a
        className="text-primary-500 text-13px font-bold py-0.5"
        href={item.url}
      >
        {item.title}
      </a>
      <h2 className="text-struct-900 text-12px font-light  py-0.5">
        {item.snippet}
      </h2>
      <h2 className="text-struct-600 text-[12px] font-light py-0.5">
        {timeAgo(item.publishedAt)}
      </h2>
    </ul>
  );
}

export function formatNews(list) {
  if (list.length === 0) {
    throw new Error("No news items found for given query.");
  }
  return list.map((item) => formatNewsItem(item));
}

/**
 * Sievo-styled list component.
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

export const List = ({ supplier, country, language, ...props }) => {
  const [replies, setReplies] = useState([]);
  const [apiResponseError, setAPIResponseError] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [hasMoreItems, setHasMoreItems] = useState(true);

  useEffect(() => {
    const abortController = new AbortController();
    if (supplier) {
      setIsLoading(true);
      setPageNumber(1);
      fetch(
        "/api/suppliernews?supplierName=" +
          encodeURIComponent(supplier) +
          (country ? "&country=" + country : "") +
          (language ? "&language=" + language.toLowerCase() : "") +
          "&pageNumber=1",
        {
          signal: abortController.signal,
        }
      )
        .then(async (response) => {
          setAPIResponseError(null);
          setReplies([]);
          const json = await response.json();
          if (!response.ok) {
            throw new Error(json.error);
          }
          if (json.length > 0) {
            setHasMoreItems(true);
          } else {
            setHasMoreItems(false);
          }
          const newsItems = formatNews(json);
          setReplies(newsItems);
        })
        .catch((error) => {
          if (error instanceof DOMException && error.name === "AbortError") {
            return;
          }
          setAPIResponseError(error.message);
        })
        .finally(() => {
          setIsLoading(false);
        });
      return () => {
        abortController.abort();
      };
    }
  }, [supplier, country, language]);

  return isLoading ? (
    <div className="flex justify-center py-20 h-full">
      <Spinner />
    </div>
  ) : (
    <div className="h-full space-y-5">
      <ul className="">{replies}</ul>
      <h2>{apiResponseError}</h2>
      <div className="px-12 space-x-2 flex justify-center">
        {supplier && hasMoreItems && (
          <Button
            onPress={() => {
              setIsLoadingMore(true);
              setPageNumber(pageNumber + 1);
              fetch(
                "/api/suppliernews?supplierName=" +
                  encodeURIComponent(supplier) +
                  (country ? "&country=" + country : "") +
                  (language ? "&language=" + language.toLowerCase() : "") +
                  "&pageNumber=" +
                  (pageNumber + 1)
              )
                .then(async (response) => {
                  setAPIResponseError(null);
                  const json = await response.json();
                  if (!response.ok) {
                    throw new Error(json.error);
                  }
                  if (json.length > 0) {
                    setHasMoreItems(true);
                  } else {
                    setHasMoreItems(false);
                  }
                  const newsItems = formatNews(json);
                  setReplies((prevReplies) =>
                    json.length === undefined
                      ? newsItems
                      : prevReplies.concat(newsItems)
                  );
                })
                .catch((error) => {
                  setAPIResponseError(error.message);
                })
                .finally(() => {
                  setIsLoadingMore(false);
                });
            }}
          >
            {isLoadingMore ? (
              <div className="flex justify-center items-center h-full">
                <Spinner />
              </div>
            ) : (
              <div className="bg-white hover:bg-gray-100 text-gray-800 font-semibold py-1 px-3 border border-gray-300 rounded shadow">
                Load More
              </div>
            )}
          </Button>
        )}
      </div>
    </div>
  );
};

export default List;
