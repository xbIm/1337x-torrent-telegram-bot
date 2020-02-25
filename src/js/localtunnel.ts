export function createLocalTunnel(port: number): Promise<string> {
    return require('ngrok').connect(port);
}
