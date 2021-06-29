namespace SystemInfo.Shared.Responses {
    public class OperationResponse<T> : EmptyOperationResponse {
        public T Record { get; set; }
    }


}
