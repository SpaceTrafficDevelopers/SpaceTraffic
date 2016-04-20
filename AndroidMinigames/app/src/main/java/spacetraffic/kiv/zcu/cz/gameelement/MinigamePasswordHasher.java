package spacetraffic.kiv.zcu.cz.gameelement;

import org.apache.commons.codec.binary.Base64;

import java.io.UnsupportedEncodingException;
import java.security.Key;
import java.security.spec.KeySpec;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;

/**
 * Minigame password hasher class.
 */
public class MinigamePasswordHasher {

    /**
     * Initialization vector.
     */
    private static String INIT_VECTOR = "05yngXV9RCNhmsmt";

    /**
     * PDKBF2 password.
     */
    private static String PDKBF2_PASSWORD = "RrapN54SFry37HXkQuym0AOa5x4HrAeKBJQGgM6E";

    /**
     * Salt.
     */
    private static String SALT = "3Z1OH3m3CpUzGIA6E3vZkPTNMtatK9C5knnhMfaa";

    /**
     * Charset.
     */
    private static final String CHARSET = "UTF-8";

    /**
     * Tranformation algorithm.
     */
    private static final String TRANSFORMATION = "AES/CBC/PKCS5Padding";

    /**
     * Key algorithm.
     */
    private static final String ALGORITHM = "PBKDF2WithHmacSHA1";

    /**
     * Method for  encryption password.
     * @param password password
     * @return encripted password
     * @throws Exception
     */
    public String getEncryptedPassword(String password) throws Exception {
        Cipher cipher = getCipher(Cipher.ENCRYPT_MODE);
        byte[] encryptedPassword = cipher.doFinal(password.getBytes(CHARSET));

        return new String(Base64.encodeBase64(encryptedPassword), CHARSET);
    }

    /**
     * Method for get original password from hashed.
     * @param encryptedPassword encrypted password
     * @return original password
     * @throws Exception
     */
    public String getOriginalPassword(String encryptedPassword) throws Exception {
        byte[] fromBase = Base64.decodeBase64(encryptedPassword.getBytes(CHARSET));

        Cipher cipher = getCipher(Cipher.DECRYPT_MODE);
        byte[] decryptedPassword = cipher.doFinal(fromBase);

        return new String(decryptedPassword);
    }

    /**
     * Method for getting Cipher.
     * @param mode mode
     * @return cipher
     * @throws Exception
     */
    private Cipher getCipher(int mode) throws Exception {
        Cipher cipher = Cipher.getInstance(TRANSFORMATION);

        byte[] initVector = INIT_VECTOR.getBytes(CHARSET);
        cipher.init(mode, generateKey(), new IvParameterSpec(initVector));

        return cipher;
    }

    /**
     * Method for generating key.
     * @return key
     * @throws Exception
     */
    private Key generateKey() throws Exception {
        SecretKeyFactory factory = SecretKeyFactory.getInstance(ALGORITHM);
        char[] pdkbf2Password = PDKBF2_PASSWORD.toCharArray();
        byte[] salt = SALT.getBytes(CHARSET);

        KeySpec keySpec = new PBEKeySpec(pdkbf2Password, salt, 65536, 128);
        SecretKey secretKey = factory.generateSecret(keySpec);
        byte[] encodedKey = secretKey.getEncoded();

        return new SecretKeySpec(encodedKey, "AES");
    }
}
