import React, { Fragment, useState, useEffect, useMemo } from "react";
import { Card, Col, Row, Button, Table } from "react-bootstrap";
import { Formik, Form, Field, ErrorMessage } from "formik";
import { useNavigate } from "react-router-dom";
import { useTable, usePagination, useSortBy } from "react-table";
import Pagination from "rc-pagination";
import { BsChevronDown, BsChevronUp } from "react-icons/bs";
import "rc-pagination/assets/index.css";
import locale from "rc-pagination/lib/locale/en_US";
import swal from "sweetalert2";
import insuranceQuoteService from "services/insuranceQuoteService";
import queryInsuranceQuoteSchema from "schemas/queryInsuranceQuoteSchema";
import "../insurancequotes/insurancequotes.css";

function InsuranceQuotes() {
  const [insuranceQuoteData, setInsuranceQuoteData] = useState({
    quotes: [],
    quoteComponents: [],
    totalCount: 0,
    pageSize: 5,
    pageIndex: 0,
  });

  const [quoteFilterData] = useState({
    query: "",
    startDate: "",
    endDate: "",
  });

  useEffect(() => {
    if (quoteFilterData.query && !quoteFilterData.startDate) {
      insuranceQuoteService
        .searchUser(
          insuranceQuoteData.pageIndex,
          insuranceQuoteData.pageSize,
          quoteFilterData.query
        )
        .then(onGetInsuranceSuccess)
        .catch(onSearchError);
    } else if (
      quoteFilterData.startDate &&
      quoteFilterData.endDate &&
      !quoteFilterData.query
    ) {
      insuranceQuoteService
        .searchDateRange(
          insuranceQuoteData.pageIndex,
          insuranceQuoteData.pageSize,
          quoteFilterData.startDate,
          quoteFilterData.endDate
        )
        .then(onGetInsuranceSuccess)
        .catch(onSearchError);
    } else if (
      quoteFilterData.query &&
      quoteFilterData.startDate &&
      quoteFilterData.endDate
    ) {
      insuranceQuoteService
        .searchByEmailAndDate(
          insuranceQuoteData.pageIndex,
          insuranceQuoteData.pageSize,
          quoteFilterData.query,
          quoteFilterData.startDate,
          quoteFilterData.endDate
        )
        .then(onGetInsuranceSuccess)
        .catch(onSearchError);
    } else
      insuranceQuoteService
        .getQuotes(insuranceQuoteData.pageIndex, insuranceQuoteData.pageSize)
        .then(onGetInsuranceSuccess)
        .catch(onGetInsuranceFail);
  }, [
    insuranceQuoteData.pageIndex,
    insuranceQuoteData.pageSize,
    quoteFilterData.query,
  ]);

  const onGetInsuranceSuccess = (response) => {
    let arrayOfQuotes = response.item.pagedItems;

    setInsuranceQuoteData((prevState) => {
      const pd = { ...prevState };
      pd.quotes = arrayOfQuotes.map((quotes) => {
        return {
          id: quotes.id,
          insuranceName: quotes.insurance.name,
          coverageStartDate: new Date(quotes.coverageStartDate).toDateString(),
          coverageEndDate: new Date(quotes.coverageEndDate).toDateString(),
          policyRange: quotes.policyRangeId.name,
          dateCreated: new Date(quotes.dateCreated).toDateString(),
          createdBy: `${quotes.createdBy.firstName} ${quotes.createdBy.lastName}`,
        };
      });
      pd.totalCount = response.item.totalCount;
      return pd;
    });
  };

  const handleSubmit = (values) => {
    if (values.query && values.startDate) {
      insuranceQuoteService
        .searchByEmailAndDate(
          0,
          insuranceQuoteData.pageSize,
          values.query,
          values.startDate,
          values.endDate
        )
        .then(onGetInsuranceSuccess)
        .catch(onSearchError);
    } else if (values.startDate !== "" || values.endDate !== "") {
      insuranceQuoteService
        .searchDateRange(
          0,
          insuranceQuoteData.pageSize,
          values.startDate,
          values.endDate
        )
        .then(onGetInsuranceSuccess)
        .catch(onSearchError);
    } else if (values.query && !values.startDate) {
      insuranceQuoteService
        .searchUser(0, insuranceQuoteData.pageSize, values.query)
        .then(onGetInsuranceSuccess)
        .catch(onSearchError);
    } else {
      insuranceQuoteService
        .getQuotes(insuranceQuoteData.pageIndex, insuranceQuoteData.pageSize)
        .then(onGetInsuranceSuccess)
        .catch(onGetInsuranceFail);
    }
  };

  const onSearchError = () => {
    swal.fire("Error", "Quotes Not Found.");
  };

  const onGetInsuranceFail = () => {
    swal.fire("Error", "Quotes Not Found.");
  };

  const navigate = useNavigate();

  const InsuranceQuoteForm = () => {
    navigate("/insurance/quotes/new");
  };

  const columns = useMemo(
    () => [
      {
        accessor: "id",
        Header: "id",
        show: false,
      },
      {
        accessor: "insuranceName",
        Header: "Insurance Name",
      },
      {
        accessor: "coverageStartDate",
        Header: "Coverage Start Date",
      },

      {
        accessor: "coverageEndDate",
        Header: "Coverage End Date",
      },
      {
        accessor: "policyRange",
        Header: "Policy Range",
      },
      {
        accessor: "dateCreated",
        Header: "Date Created",
      },
      {
        accessor: "createdBy",
        Header: "Created By",
      },
    ],
    []
  );

  const data = insuranceQuoteData.quotes;

  const { getTableProps, getTableBodyProps, headerGroups, page, prepareRow } =
    useTable(
      {
        columns,
        data,
        initialState: {
          pageSize: insuranceQuoteData.pageSize,
          hiddenColumns: columns.map((column) => {
            if (column.show === false) return column.accessor || column.id;
            else return false;
          }),
        },
      },
      useSortBy,
      usePagination
    );

  const changePageClicked = (page) => {
    setInsuranceQuoteData((prev) => {
      let pd = { ...prev };
      pd.pageIndex = page - 1;
      return pd;
    });
  };
  return (
    <Fragment>
      <Card className="border-0">
        <Card.Header>
          <Row>
            <Col sm>
              <h2 className="mb-0">Insurance Quotes</h2>
            </Col>
            <Col sm>
              <Button
                id="addInsuranceQuote"
                variant="btn btn-dark"
                type="submit"
                className="addInsuranceQuote"
                onClick={InsuranceQuoteForm}
              >
                Add An Insurance Quote
              </Button>
            </Col>
          </Row>
        </Card.Header>
        <Formik
          enableReinitialize={true}
          initialValues={quoteFilterData}
          onSubmit={handleSubmit}
          validationSchema={queryInsuranceQuoteSchema}
        >
          <Form>
            <div className="row">
              <Col lg>
                <span className="dropdown-header px-0 mt-5 mt-lg-0 ms-lg-3 ">
                  Search
                </span>
                <div className="mt-5 mt-lg-0 ms-lg-3 d-flex align-items-center">
                  <Field
                    as="input"
                    id="query"
                    name="query"
                    className="form-control"
                  />
                  <ErrorMessage
                    name="query"
                    component="div"
                    className="has-error"
                  />
                </div>
              </Col>
              <Col>
                <span className="dropdown-header px-0 mt-5 mt-lg-0 ms-lg-3 ">
                  Select Date Range
                </span>
                <div className="mt-5 mt-lg-0 ms-lg-3 d-flex align-items-center">
                  <div className="px-3">
                    <Field
                      type="date"
                      id="startDate"
                      name="startDate"
                      className="form-control"
                    />
                    <ErrorMessage
                      name="startDate"
                      component="Row"
                      className="has-error"
                    />
                  </div>
                  <div className="px-3">
                    <Field
                      type="date"
                      id="endDate"
                      name="endDate"
                      className="form-control"
                    />
                  </div>
                </div>
              </Col>
              <Col sm>
                <button className="btn btn-dark btn-sm mt-5" type="submit">
                  Filter
                </button>
              </Col>
            </div>
          </Form>
        </Formik>

        <Card.Body className="p-0 pb-5">
          <Row>
            <div className="table-quotes table-responsive border-0">
              <Table {...getTableProps()} className="table m-0 text-nowrap">
                <thead className="table-light">
                  {headerGroups.map((headerGroup) => (
                    <tr key={1} {...headerGroup.getHeaderGroupProps()}>
                      {headerGroup.headers.map((column) => (
                        <th
                          key={1}
                          {...column.getHeaderProps(
                            column.getSortByToggleProps()
                          )}
                        >
                          {column.render("Header")}
                          <span>
                            {column.isSorted ? (
                              column.isSortedDesc ? (
                                <BsChevronDown
                                  size="16px"
                                  className="ms-lg-1"
                                  color="#19cb98"
                                />
                              ) : (
                                <BsChevronUp
                                  size="16px"
                                  className="ms-lg-1"
                                  color="#19cb98"
                                />
                              )
                            ) : (
                              ""
                            )}
                          </span>
                        </th>
                      ))}
                    </tr>
                  ))}
                </thead>
                <tbody {...getTableBodyProps()}>
                  {page.map((row) => {
                    prepareRow(row);
                    return (
                      <tr key={1} {...row.getRowProps()}>
                        {row.cells.map((cell) => {
                          return (
                            <td key={1} {...cell.getCellProps()}>
                              {cell.render("Cell")}
                            </td>
                          );
                        })}
                      </tr>
                    );
                  })}
                </tbody>
              </Table>
            </div>
          </Row>
          <center>
            <Pagination
              locale={locale}
              onChange={changePageClicked}
              current={insuranceQuoteData.pageIndex + 1}
              total={insuranceQuoteData.totalCount}
              pageSize={insuranceQuoteData.pageSize}
            />
          </center>
        </Card.Body>
      </Card>
    </Fragment>
  );
}

export default InsuranceQuotes;
