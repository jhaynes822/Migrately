import axios from "axios";
import {
  onGlobalSuccess,
  onGlobalError,
  API_HOST_PREFIX,
} from "./serviceHelpers";

const endpoint = {
  insuranceUrl: `${API_HOST_PREFIX}/api/insurances/quotes/`,
  countriesUrl: `${API_HOST_PREFIX}/api/insurances/quotes/country`,
};

const addInsuranceQuote = (payload) => {
  const config = {
    method: "POST",
    url: `${endpoint.insuranceUrl}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getCitizenCountries = () => {
  const config = {
    method: "Get",
    url: `${endpoint.countriesUrl}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getInsurance = () => {
  const config = {
    method: "Get",
    url: `${endpoint.insuranceUrl}insurance`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getQuotes = (pageIndex, pageSize) => {
  const config = {
    method: "Get",
    url: `${endpoint.insuranceUrl}paginate?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const searchUser = (pageIndex, pageSize, user) => {
  const config = {
    method: "Get",
    url: `${endpoint.insuranceUrl}createdby/paginate?pageIndex=${pageIndex}&pageSize=${pageSize}&user=${user}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const searchDateRange = (pageIndex, pageSize, dateRangeStart, dateRangeEnd) => {
  const config = {
    method: "Get",
    url: `${endpoint.insuranceUrl}daterange/paginate?pageIndex=${pageIndex}&pageSize=${pageSize}&dateRangeStart=${dateRangeStart}&dateRangeEnd=${dateRangeEnd}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const searchByUserAndDate = (
  pageIndex,
  pageSize,
  user,
  dateRangeStart,
  dateRangeEnd
) => {
  const config = {
    method: "Get",
    url: `${endpoint.insuranceUrl}createdby/daterange/paginate?pageIndex=${pageIndex}&pageSize=${pageSize}&dateRangeStart=${dateRangeStart}&dateRangeEnd=${dateRangeEnd}&user=${user}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const insuranceQuoteService = {
  addInsuranceQuote,
  getInsurance,
  getCitizenCountries,
  getQuotes,
  searchUser,
  searchDateRange,
  searchByUserAndDate,
};

export default insuranceQuoteService;
