import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import { Resource } from '@opentelemetry/resources';
import './index.css'

import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { BatchSpanProcessor, TracerConfig, WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-proto';
import { SemanticResourceAttributes } from '@opentelemetry/semantic-conventions';
//import { HttpInstrumentation } from '@opentelemetry/instrumentation-http';

const collectorOptions = {
  url: 'http://host.docker.internal:4318/v1/traces', // <-- nginx with reverse proxy to http://localhost:4318/
};

// const collectorExporter = new OTLPTraceExporter({

//   headers: {}

// });

const providerConfig: TracerConfig = {
  resource: new Resource({
    [SemanticResourceAttributes.SERVICE_NAME]: 'frontend-react',
    [SemanticResourceAttributes.SERVICE_VERSION]: '1.0.0',
  }),
};

const provider = new WebTracerProvider(providerConfig);
provider.addSpanProcessor(
  new BatchSpanProcessor(new OTLPTraceExporter(collectorOptions)),
);
//provider.addSpanProcessor(new SimpleSpanProcessor(new ConsoleSpanExporter()));
//provider.addSpanProcessor(new SimpleSpanProcessor(collectorExporter));
//provider.addSpanProcessor(new SimpleSpanProcessor(collectorExporter));
provider.register({
  contextManager: new ZoneContextManager(),
});

const fetchInstrumentation = new FetchInstrumentation({});
fetchInstrumentation.setTracerProvider(provider);

registerInstrumentations({
  instrumentations: [
    // getWebAutoInstrumentations initializes all the package.
	// it's possible to configure each instrumentation if needed.
  fetchInstrumentation,
  // new HttpInstrumentation(),
  ],
  tracerProvider: provider
});

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
