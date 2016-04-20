package spacetraffic.kiv.zcu.cz.logoquiz;

import android.util.Log;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import java.io.IOException;

/**
 * Class for communication with server via SOAP.
 */
public class Sender {

    /**
     * Method name.
     */
    private String methodName;

    /**
     * Request object.
     */
    private SoapObject request;

    /**
     * Service NAMESPACE.
     */
    private static final String NAMESPACE = "http://spacetraffic.kiv.zcu.cz/MinigameService";

    /**
     * Service soap action without name.
     */
    private static final String SOAP_ACTION = "http://spacetraffic.kiv.zcu.cz/MinigameService/IMinigameService/";

    /**
     * Url to webservice to server.
     */
    private static final String URL = "http://10.0.2.2:8080/SpaceTraffic/Minigame";

    /**
     * Soap version.
     */
    private static final int SOAP_VERSION = SoapEnvelope.VER11;

    /**
     * Sender constructor.
     * @param methodName method name
     */
    public Sender(String methodName){
        this.methodName = methodName;
        this.request = new SoapObject(NAMESPACE, methodName);
    }

    /**
     * Method for adding property.
     * @param parameterName parameter name
     * @param value value
     */
    public void addProperty(String parameterName, Object value){
        this.request.addProperty(parameterName, value);
    }

    /**
     * Method for adding property info.
     * @param propertyInfo property info
     */
    public void addProperty(PropertyInfo propertyInfo){
        this.request.addProperty(propertyInfo);
    }

    /**
     * Method for calling soap method on server.
     * @return returns result from server or nul
     */
    public Object callMethod(){
        SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SOAP_VERSION);

        envelope.implicitTypes = true;
        envelope.dotNet = true;
        envelope.setOutputSoapObject(request);

        HttpTransportSE androidHttpTransport = new HttpTransportSE(URL);

        try {
            androidHttpTransport.call(SOAP_ACTION + methodName, envelope);

            return envelope.getResponse();
        } catch (Exception e){
            Log.e(this.getClass().getSimpleName(), e.getMessage());
            return null;
        }
    }

}
