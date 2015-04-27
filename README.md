# XPlaneGen

## Purpose
The purpose of this solution is to store flight data in a structured binary format, instead of text-based formats.

## Processing
__Status__: Alpha

Compute-bound processes are sped-up by converting textual data from source files into an intermediate binary form. Flight data is typically not structured in an efficient manner, relegating any analysis to inefficient linear searches. Significant lookup speeds are attained due to efficient structuring. Additional, LINQ queries (see XPlaneQuery) are made possible due to this action as processed data is strongly-typed.

## XPlane Flight Data Recorder
__Status__: Design

Processed data can be converted into a FDR file (a CSV file) for XPlane to playback.

## XPlaneQuery
__Status__: Experimental

This module allows for analysis of data by using LINQ expressions. A lightweight expression parsing library used for converting string inputs into lambda expressions. These expressions are used to provide enhanced conversions for datapoints (e.g. converting a processed datapoint into an XPlane FDR datapoint). Current design is only for a very small subset of syntax (e.g. limited method invocation, and readonly access).

## Limitations
Currently, only one specific format is supported.
